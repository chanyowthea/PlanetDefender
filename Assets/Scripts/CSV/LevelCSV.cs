using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCSV : CSVBaseData
{
    public int _Level;
    public int _TargetScore;

    public override string GetPrimaryKey()
    {
        return _Level.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _Level = ReadInt("Level", headers, values);
        _TargetScore = ReadInt("TargetScore", headers, values);
    }
}
