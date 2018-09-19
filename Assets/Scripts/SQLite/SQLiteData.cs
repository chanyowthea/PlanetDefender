using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// support int, string, float, bool only. 
public abstract class SQLiteData
{
    protected static FieldInfo[] _FieldInfos;

    public SQLiteData()
    {
        if (_FieldInfos == null)
        {
            _FieldInfos = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        }
    }

    public SqliteParameter[] ToSqliteParams()
    {
        List<SqliteParameter> ps = new List<SqliteParameter>();
        for (int i = 0, length = _FieldInfos.Length; i < length; ++i)
        {
            var field = _FieldInfos[i];
            ps.Add(new SqliteParameter(field.Name, field.GetValue(this)));
        }
        return ps.ToArray();
    }

    public string[] ToSqliteTypes()
    {
        string[] types = new string[_FieldInfos.Length];
        for (int i = 0, length = _FieldInfos.Length; i < length; ++i)
        {
            var field = _FieldInfos[i];
            types[i] = SQLiteUtil.ToSqliteType(field.FieldType);
        }
        return types; 
    }
}
