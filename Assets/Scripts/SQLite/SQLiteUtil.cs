using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum ESQLiteAttributeType
{
    None,
    PrimaryKey,
    PrimaryKeyAutoIncrement,
    Unique,
}

public static class SQLiteUtil
{
    public static string ToSqliteType(this Type t, ESQLiteAttributeType attributeType = ESQLiteAttributeType.None)
    {
        StringBuilder sb = new StringBuilder();
        if (t.ToString() == "System.Int32")
        {
            sb.Append("INTEGER");
        }
        else if (t.ToString() == "System.Single")
        {
            sb.Append("REAL");
        }
        else if (t.ToString() == "System.Byte[]")
        {
            sb.Append("BLOB");
        }
        else // (t.ToString() == "System.String")
        {
            sb.Append("TEXT");
        }
        switch (attributeType)
        {
            case ESQLiteAttributeType.PrimaryKey:
                sb.Append(" PRIMARY KEY");
                break;
            case ESQLiteAttributeType.PrimaryKeyAutoIncrement:
                sb.Append(" PRIMARY KEY AUTOINCREMENT");
                break;
            case ESQLiteAttributeType.Unique:
                sb.Append(" UNIQUE");
                break;
        }
        return sb.ToString();
    }

    public static object GetValueFromSQLite(this object obj, Type fieldType)
    {
        if (obj == null)
        {
            return default(object);
        }
        string s = ""; 
        if (fieldType.ToString() == "System.Int32")
        {
            int rs = 0; 
            int.TryParse(obj.ToString(), out rs);
            return rs as object; 
        }
        else if (fieldType.ToString() == "System.Single")
        {
            float rs = 0;
            float.TryParse(obj.ToString(), out rs);
            return rs as object;
        }
        else if (fieldType.ToString() == "System.Byte[]")
        {
            // TODO waiting for test
            return obj as byte[];
        }
        else
        {
            return obj.ToString(); 
        }
    }
}
