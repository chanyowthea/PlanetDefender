using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// support int, string, float, byte[] only. 
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

    public string[] ToSqliteTypes(Dictionary<string, ESQLiteAttributeType> extraAttributes = null)
    {
        string[] types = new string[_FieldInfos.Length];
        for (int i = 0, length = _FieldInfos.Length; i < length; ++i)
        {
            var field = _FieldInfos[i];
            if (extraAttributes != null && extraAttributes.ContainsKey(field.Name))
            {
                types[i] = SQLiteUtil.ToSqliteType(field.FieldType, extraAttributes[field.Name]);
            }
            else
            {
                types[i] = SQLiteUtil.ToSqliteType(field.FieldType);
            }
        }
        return types;
    }

    public string[] ToFieldNames()
    {
        string[] names = new string[_FieldInfos.Length];
        for (int i = 0, length = _FieldInfos.Length; i < length; ++i)
        {
            var field = _FieldInfos[i];
            names[i] = field.Name;
        }
        return names;
    }
    
    // the order of reading is correct so that the order of setting values is correct too. 
    public void SetAllValues(List<object> objs)
    {
        var fs = _FieldInfos;
        if (fs.Length != objs.Count)
        {
            Debugger.LogError(string.Format("the length of data is inconsistent with the number of variables！variables count：{0}, data count：{1}", 
                fs.Length, objs.Count));
            return;
        }
        for (int i = 0, max = objs.Count; i < max; i++)
        {
            var f = fs[i];
            f.SetValue(this, objs[i].GetValueFromSQLite(f.FieldType));
        }
    }
}
