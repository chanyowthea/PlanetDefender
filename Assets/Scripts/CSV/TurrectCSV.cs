using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurrectCSV : CSVBaseData
{
    public int _ID;
    public string _Name;
    public int _Attack;
    public int _Defense;
    public string _Picture;

    public override string GetPrimaryKey()
    {
        return _ID.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _ID = ReadInt("ID", headers, values);
        _Name = ReadString("Name", headers, values);
        _Attack = ReadInt("Attack", headers, values); 
        _Defense = ReadInt("Defense", headers, values);
        _Picture = ReadString("Picture", headers, values);
    }
}
