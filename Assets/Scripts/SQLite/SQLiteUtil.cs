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
    IsUnique,
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
            sb.Append("FLOAT");
        }
        else if (t.ToString() == "System.Boolean")
        {
            sb.Append("BIT");
        }
        else // (t.ToString() == "System.String")
        {
            sb.Append("TEXT");
        }
        switch (attributeType)
        {
            case ESQLiteAttributeType.PrimaryKey:
                sb.Append("PRIMARY KEY");
                break;
            case ESQLiteAttributeType.PrimaryKeyAutoIncrement:
                sb.Append("PRIMARY KEY AUTOINCREMENT");
                break;
            case ESQLiteAttributeType.IsUnique:
                sb.Append("UNIQUE");
                break;
        }
        return sb.ToString();
    }
}
