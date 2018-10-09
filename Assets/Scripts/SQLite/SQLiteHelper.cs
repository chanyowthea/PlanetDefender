using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class SQLiteHelper
{
    string connectionString = "";
    const char _Identifier = '@';
    public SQLiteHelper(string database)
    {
        string dir = "";
#if UNITY_EDITOR
        dir = Application.dataPath + "/DataBase/";
        connectionString = "data source=" + dir + string.Format("{0}.db", database);
#elif UNITY_ANDROID
        dir = Application.persistentDataPath + "/DataBase/"; 
        connectionString = "URI=file:" + dir + string.Format("{0}.db", database);
#endif
        Debugger.Log("dir=" + dir);
        Debugger.Log("connectionString=" + connectionString);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
    }

    public void ExecuteNonQuery(string queryString)
    {
        using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
        {
            try
            {
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.LogError("ExecuteNonQuery ex.Message=" + e.Message);
            }

            using (SqliteCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = queryString;
                try
                {
                    dbCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Debug.LogError("ExecuteNonQuery ex.Message=" + ex.Message);
                }
            }
        }
    }

    public List<List<object>> ExecuteReader(string queryString)
    {
        var result = new List<List<object>>();
        using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
        {
            try
            {
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }

            using (SqliteCommand command = dbConnection.CreateCommand())
            {
                command.CommandText = queryString;
                try
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 参数个数
                            var buffer = new object[reader.FieldCount];
                            // 读取所有值
                            reader.GetValues(buffer);
                            result.Add(buffer.ToList());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("ExecuteQuery ex.Message=" + ex.Message);
                }
            }
        }
        return result;
    }

    // 获取表中符合条件的记录数
    public int GetCount(string tableName, params SqliteParameter[] args)
    {
        // conditions
        string temp = "";
        for (int i = 0; i < args.Length; ++i)
        {
            temp += args[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[i].Value.ToString()) + ((i != args.Length - 1) ? " AND " : "");
        }

        string format = (args == null || args.Length == 0) ? "SELECT COUNT(*) FROM {0}" : "SELECT COUNT(*) FROM {0} WHERE {1}";
        var list = ExecuteReader(string.Format(format, tableName, temp));

        if (list == null || list.Count == 0 || list[0].Count == 0 || (list[0][0]).ToString() == "0")
        {
            return 0;
        }
        return list.Count;
    }

    /// <summary>
    /// 根据条件读取数据，并且返回对应条数的数据组，前面的List是数据行数，后面的List是列名对应的数据
    /// </summary>
    /// <param name="columnNames">需要读取的字段</param>
    /// <param name="conditions">读取信息的限定条件</param>
    public List<List<object>> ReaderInfo(string tableName, string[] columnNames, params SqliteParameter[] conditions)
    {
        if (columnNames == null || columnNames.Length == 0)
        {
            return null;
        }

        // columns name
        string temp0 = "";
        for (int i = 0; i < columnNames.Length; ++i)
        {
            temp0 += ", " + columnNames[i];
        }
        temp0 = temp0.TrimStart(',', ' ');

        // conditions
        string temp1 = "";
        for (int i = 0; i < conditions.Length; ++i)
        {
            // name=@name
            temp1 += conditions[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(conditions[i].Value.ToString()) 
                + ((i != conditions.Length - 1) ? " AND " : "");
        }

        string format = (conditions == null || conditions.Length == 0) ? "SELECT {0} FROM {1}" : "SELECT {0} FROM {1} WHERE {2}";
        return ExecuteReader(string.Format(format, temp0, tableName, temp1));
    }

    public List<List<object>> ReadAllInfo(string tableName, params SqliteParameter[] args)
    {
        // conditions
        string temp = "";
        for (int i = 0; i < args.Length; ++i)
        {
            temp += args[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[i].Value.ToString()) + ((i != args.Length - 1) ? " AND " : "");
        }

        string format = (args == null || args.Length == 0) ? "SELECT * FROM {0}" : "SELECT * FROM {0} WHERE {1}";
        return ExecuteReader(string.Format(format, tableName, temp));
    }

    // 插入数据
    void InsertInto(string tableName, params SqliteParameter[] args)
    {
        if (args == null || args.Length <= 0)
        {
            return;
        }
        string format = "INSERT INTO {0} ({1}) VALUES ({2})";
        // columns name
        string temp0 = "";
        for (int i = 0; i < args.Length; ++i)
        {
            temp0 += ", " + args[i].ParameterName.TrimStart(_Identifier);
        }
        temp0 = temp0.TrimStart(',', ' ');
        
        // values
        string temp1 = "";
        for (int i = 0; i < args.Length; ++i)
        {
            temp1 += ", " + GetStringForSQL(args[i].Value.ToString());
        }
        temp1 = temp1.TrimStart(',', ' ');
        ExecuteNonQuery(string.Format(format, tableName, temp0, temp1));
    }
    
    public void UpdateValues(string tableName, SqliteParameter condition, params SqliteParameter[] args)
    {
        // 如果没有信息就插入信息
        var count = GetCount(tableName, condition);
        if (count == 0)
        {
            InsertInto(tableName, args);
            return;
        }

        string format = "UPDATE {0} SET {1} WHERE {2}";

        // set values 
        string temp0 = "";
        for (int i = 0; i < args.Length; ++i)
        {
            temp0 += args[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[i].Value.ToString()) + ((i != args.Length - 1) ? ", " : "");
        }

        // condition
        string temp1 = condition.ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(condition.Value.ToString());
        ExecuteNonQuery(string.Format(format, tableName, temp0, temp1));
    }

    public void DeleteValuesOR(string tableName, params SqliteParameter[] args)
    {
        if(args == null)
            { 
            return; 
            }
        string queryString = "DELETE FROM " + tableName + " WHERE " + args[0].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[0].Value.ToString());
        for (int i = 1; i < args.Length; i++)
        {
            queryString += "OR " + args[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[i].Value.ToString());
        }
        ExecuteNonQuery(queryString);
    }

    public void DeleteValuesAND(string tableName, params SqliteParameter[] args)
    {
        if (args == null)
        {
            return;
        }
        string queryString = "DELETE FROM " + tableName + " WHERE " + args[0].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[0].Value.ToString());
        for (int i = 1; i < args.Length; i++)
        {
            queryString += "AND " + args[i].ParameterName.TrimStart(_Identifier) + "=" + GetStringForSQL(args[i].Value.ToString());
        }
        ExecuteNonQuery(queryString);
    }

    /// <summary>
    /// 创建数据表
    /// </summary> +
    /// <param name="tableName">数据表名</param>
    /// <param name="colNames">字段名</param>
    /// <param name="colTypes">字段名类型</param>
    public void CreateTable(string tableName, string[] colNames, string[] colTypes)
    {
        string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
        for (int i = 1; i < colNames.Length; i++)
        {
            queryString += ", " + colNames[i] + " " + colTypes[i];
        }
        queryString += "  ) ";
        ExecuteNonQuery(queryString);
    }

    public void DeleteTable(string tableName)
    {
        string queryString = "DROP TABLE " + tableName;
        ExecuteNonQuery(queryString);
    }

    string GetStringForSQL(string content)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("'");
        sb.Append(content);
        sb.Append("'");
        return sb.ToString();
    }
}