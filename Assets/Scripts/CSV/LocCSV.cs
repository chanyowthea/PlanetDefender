using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocCSV : CSVBaseData
{
    public string _ID;
    public string _English;
    public string _Chinese;
    public string _TraditionalChinese;
    public string _Japanese;

    public override string GetPrimaryKey()
    {
        return _ID.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _ID = ReadString("ID", headers, values);
        _English = ReadString("English", headers, values);
        _Chinese = ReadString("Chinese", headers, values);
        _TraditionalChinese = ReadString("TraditionalChinese", headers, values);
        _Japanese = ReadString("Japanese", headers, values);
    }
}
