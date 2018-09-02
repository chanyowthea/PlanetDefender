using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

public class Sqlite
{
    //Sqlite连接前缀
    private const string DB_CONNECTION_PREFIX = "URI=file:";
    //连接器
    private IDbConnection m_dbConn;
    //查询命令
    private IDbCommand m_command;
    //事物
    private IDbTransaction m_dbTrans;


    public Sqlite(string path)
    {
        OpenDataBase(path);
    }

    private void OpenDataBase(string path)
    {
        m_dbConn = new SqliteConnection(DB_CONNECTION_PREFIX + path);
        m_dbConn.Open();
        m_command = m_dbConn.CreateCommand();
    }


    //查询函数
    //IConfig是自定义的配置类接口，最好使数据库中的配置类都继承同一个接口，方便以后扩展
    public IEnumerable ExcuteSelectQuery<T>(string sqlQuery) where T : IConfig
    {
        return ExcuteSelectQuery(sqlQuery, typeof(T));
    }


    //查询函数,这里使用反射 反序列数据，简化对对象的赋值操作
    public IEnumerable ExcuteSelectQuery(string sqlQuery, Type type)
    {
        //使用事物
        BeginTrans();
        //查询语句
        m_command.CommandText = sqlQuery;
        //查询结果
        IDataReader reader = m_command.ExecuteReader();

        //反序列化操作 所定义的变量
        PropertyInfo[] newpropertys = null;
        PropertyInfo[] oldpropertys = null;
        bool init = false;

        while (reader.Read())
        {
            //创建类对象
            IConfig config = Activator.CreateInstance(type) as IConfig;
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (!init)
                {
                    //newpropertys将类的属性 顺序的对应到数据库中的列名，方便后续的赋值操作
                    if (newpropertys == null)
                    {
                        newpropertys = new PropertyInfo[reader.FieldCount];
                    }
                    //获取类的所有属性
                    if (oldpropertys == null)
                    {
                        oldpropertys = type.GetProperties(BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
                    }
                    //当前数据的列名
                    string filedName = reader.GetName(i);

                    bool find = false;
                    PropertyInfo pro = null;
                    //查找与列名相同的自定义特性名称，相同则把查询到的数据值赋值到对象中
                    for (int j = 0; j < oldpropertys.Length; j++)
                    {
                        pro = oldpropertys[j];
                        //获取自定义特性
                        object[] objs = pro.GetCustomAttributes(typeof(ConfigFieldAttribute), false);
                        if (objs.Length == 0)
                        {
                            continue;
                        }
                        //特性的列名是否与数据库中列名相同
                        ConfigFieldAttribute cfgfield = objs[0] as ConfigFieldAttribute;
                        if (cfgfield.filedName == filedName)
                        {
                            find = true;
                            break;
                        }
                    }
                    newpropertys[i] = find ? pro : null;
                }
                //已经排好序的类属性数组
                PropertyInfo info = newpropertys[i];
                if (info == null)
                {
                    continue;
                }
                //对象的属性赋值
                info.SetValue(config, reader.GetValue(i), null);
            }

            if (!init)
            {
                init = true;
            }
            yield return config;
        }
        Commit();
        reader.Close();
    }

    //使用事物
    public void BeginTrans()
    {
        m_dbTrans = m_dbConn.BeginTransaction();
        m_command.Transaction = m_dbTrans;
    }

    //事物回滚
    public void Rollback()
    {
        m_dbTrans.Rollback();
    }

    //事物生效
    public void Commit()
    {
        m_dbTrans.Commit();
    }

    //执行其他sql语句
    public void ExcuteQuery(string sqlQuery)
    {
        m_command.CommandText = sqlQuery;
        m_command.ExecuteNonQuery();
    }


    //关闭连接
    public void Close()
    {
        if (m_dbTrans != null)
        {
            m_dbTrans.Dispose();
            m_dbTrans = null;
        }
        if (m_command != null)
        {
            m_command.Dispose();
            m_command = null;
        }
        m_dbConn.Close();
        m_dbConn = null;
    }
}