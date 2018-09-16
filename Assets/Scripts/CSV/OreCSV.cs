using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCSV : CSVBaseData
{
    public int _ID;
    public string _Name;
    public string _Picture; 
    public int _Price;
    public int _TechLevel;
    public bool _InMall;

    public override string GetPrimaryKey()
    {
        return _ID.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _ID = ReadInt("ID", headers, values);
        _Name = ReadString("Name", headers, values);
        _Picture = ReadString("Picture", headers, values);
        _Price = ReadInt("Price", headers, values);
        _TechLevel = ReadInt("TechLevel", headers, values);
        _InMall = ReadInt("InMall", headers, values) == 0;
    }
}
