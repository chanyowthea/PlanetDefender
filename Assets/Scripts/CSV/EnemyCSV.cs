using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1-废弃物;2-炸弹;3-毒药;4-燃烧品;5-外来生物;6-外星人;7-陨石
public enum EEnemyType
{
    Garbage = 1,
    Bomb,
    Poision,
    Blazer,
    Biome,
    Alien,
    Aerolite
}

public class EnemyCSV : CSVBaseData
{
    public int _ID;
    public string _Name;
    public EEnemyType _Type;
    public int _Attack;
    public int _Defense;
    public string _Picture;
    public int _Reward;
    public int _TechLevel;
    public int _MaxHP;
    public int _Weight;

    public override string GetPrimaryKey()
    {
        return _ID.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _ID = ReadInt("ID", headers, values);
        _Name = ReadString("Name", headers, values);
        _Type = (EEnemyType)ReadInt("Type", headers, values);
        _Attack = ReadInt("Attack", headers, values); 
        _Defense = ReadInt("Defense", headers, values);
        _Picture = ReadString("Picture", headers, values);
        _Reward = ReadInt("Reward", headers, values);
        _TechLevel = ReadInt("TechLevel", headers, values);
        _MaxHP = ReadInt("MaxHP", headers, values);
        _Weight = ReadInt("Weight", headers, values);
    }
}
