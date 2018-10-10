using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using System.Text;

public static class LogUtil
{
    public static string GetCurMethodName()
    {
        return new System.Diagnostics.StackFrame(1).GetMethod().Name;
    }

    public static string GetVarName<T>(Expression<Func<string, T>> exp)
    {
        return ((MemberExpression)exp.Body).Member.Name;
    }

    public static string GetLogString(this List<List<object>> list)
    {
        string s = "";
        for (int i = 0, max = list.Count; i < max; i++)
        {
            var l = list[i];
            string temp = "";
            for (int j = 0, max1 = l.Count; j < max1; j++)
            {
                temp += l[j] + (j == max1 - 1 ? "" : ", ");
            }
            s += (i == 0 ? "" : "; ") + temp;
        }
        return s;
    }

    public static string GetLogString(this object[] list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0, max = list.Length; i < max; i++)
        {
            var l = list[i];
            sb.Append((i == 0 ? "" : "; ") + l.ToString());
        }
        return sb.ToString();
    }

    public static string GetLogString(this List<object> list)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0, max = list.Count; i < max; i++)
        {
            var l = list[i];
            sb.Append((i == 0 ? "" : "; ") + l.ToString());
        }
        return sb.ToString();
    }

    public static string GetLogString(this Dictionary<int, int> dict)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var item in dict)
        {
            sb.Append(item.Key);
            sb.Append("-");
            sb.Append(item.Value);
            sb.Append(";");
        }
        sb.Remove(sb.Length - 1, 1);
        return sb.ToString(); 
    }

    public static Dictionary<int, int> GetDictionary(this string str)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        var array = str.Split(';');
        for (int i = 0, length = array.Length; i < length; i++)
        {
            var kv = array[i].Split('-');
            dict.Add(int.Parse(kv[0]), int.Parse(kv[1]));
        }
        return dict;
    }

    public static string GetHierarchy(GameObject obj)
    {
        if (obj == null) return "";
        string path = obj.name;

        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = obj.name + "\\" + path;
        }
        return path;
    }
}
