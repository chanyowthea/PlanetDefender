using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Data.Sqlite;

// 2015年11月3日更新内容：在不同的平台上数据库的存储位置是不同的，在这里给出一个参考的路径，希望大家在处理移动端的时候注意这些问题啊！
// 各平台下数据库存储的绝对路径(通用)
// PC：sql = new SQLiteHelper("data source=" + Application.dataPath + "/sqlite4unity.db");
// Mac：sql = new SQLiteHelper("data source=" + Application.dataPath + "/sqlite4unity.db");
// Android：sql = new SQLiteHelper("URI=file:" + Application.persistentDataPath + "/sqlite4unity.db");
// iOS：sql = new SQLiteHelper("data source=" + Application.persistentDataPath + "/sqlite4unity.db");
   
// PC平台下的相对路径
// sql = new SQLiteHelper("data source="sqlite4unity.db");
// 编辑器：Assets/sqlite4unity.db
// 编译后：和AppName.exe同级的目录下，这里比较奇葩
// 当然可以用更随意的方式sql = new SQLiteHelper("data source="D://SQLite//sqlite4unity.db");
// 确保路径存在即可否则会发生错误
   
// 如果是事先创建了一份数据库
// 可以将这个数据库放置在StreamingAssets目录下然后再拷贝到
// Application.persistentDataPath + "/sqlite4unity.db"路径即可

public class SQLiteDemo : MonoBehaviour
{
    /// <summary>
    /// SQLite数据库辅助类
    /// </summary>
    private SQLiteHelper sql;

    void Start()
    {

        //创建名为sqlite4unity的数据库
        sql = new SQLiteHelper("sqlite4unity");
        int c = sql.GetCount("table1", new SqliteParameter("@" + "ID", "0"));
        Debug.Log("count=" + c); 

        //sql.DeleteTable("table1");

        //创建名为table1的数据表
        sql.CreateTable("table1", new string[] { "ID", "Name", "Age", "Email" }, 
            new string[] { "INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT UNIQUE", "INTEGER", "TEXT" });

        //插入两条数据
        //sql.InsertValues("table1", new string[] { "'1'", "'张三'", "'22'", "'Zhang3@163.com'" });
        //sql.InsertValues("table1", new string[] { "'2'", "'李四'", "'25'", "'Li4@163.com'" });
        return;

        var count = sql.GetCount("table1", new SqliteParameter("@Age", 25)); 
        Debug.Log("count=" + count);

        //更新数据，将Name="张三"的记录中的Name改为"Zhang3"
        //sql.UpdateValues("table1", new string[] { "Name" }, new string[] { "'Zhang3'" }, "Name", "=", "'张三'");

        //插入3条数据
        //sql.InsertValues("table1", new string[] { "3", "'王五'", "25", "'Wang5@163.com'" });
        //sql.InsertValues("table1", new string[] { "4", "'王五'", "26", "'Wang5@163.com'" });
        //sql.InsertValues("table1", new string[] { "5", "'王五1'", "27", "'Wang5@163.com'" });

        //删除Name="王五"且Age=26的记录,DeleteValuesOR方法类似
        //sql.DeleteValuesAND("table1", new string[] { "Name", "Age" }, new string[] { "=", "=" }, new string[] { "'王五'", "'26'" });

        //读取整张表
        var reader = sql.ReadAllInfo("table1");
        //while (reader.Read())
        //{
        //    //读取ID
        //    Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
        //    //读取Name
        //    Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
        //    //读取Age
        //    Debug.Log(reader.GetInt32(reader.GetOrdinal("Age")));
        //    //读取Email
        //    Debug.Log(reader.GetString(reader.GetOrdinal("Email")));
        //}

        //读取数据表中Age>=25的所有记录的ID和Name
        //reader = sql.ReadTable("table1", new string[] { "ID", "Name" }, new string[] { "Age" }, new string[] { ">=" }, new string[] { "'25'" });
        //while (reader.Read())
        //{
        //    //读取ID
        //    Debug.Log(reader.GetInt32(reader.GetOrdinal("ID")));
        //    //读取Name
        //    Debug.Log(reader.GetString(reader.GetOrdinal("Name")));
        //}

        ////自定义SQL,删除数据表中所有Name="王五"的记录
        //sql.ExecuteQuery("DELETE FROM table1 WHERE NAME='王五'");
    }
}