using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCSV : CSVBaseData
{
    public int _Level;
    public int _Defense;
    public int _MaxHP;

    public override string GetPrimaryKey()
    {
        return _Level.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _Level = ReadInt("Level", headers, values);
        _Defense = ReadInt("Defense", headers, values);
        _MaxHP = ReadInt("MaxHP", headers, values);
    }
}
