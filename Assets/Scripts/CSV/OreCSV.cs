using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCSV : CSVBaseData
{
    public string _ID;
    public string _Name;
    public string _Picture; 
    public string _Price;
    public string _TechLevel;
    public string _PlanetRarity;
    public string _AetherRarity;

    public override string GetPrimaryKey()
    {
        return _ID.ToString();
    }

    public override void ParseData(long index, int fieldCount, string[] headers, string[] values)
    {
        _ID = ReadString("ID", headers, values);
        _Name = ReadString("Name", headers, values);
        _Picture = ReadString("Picture", headers, values);
        _Price = ReadString("Price", headers, values);
        _TechLevel = ReadString("TechLevel", headers, values);
        _PlanetRarity = ReadString("PlanetRarity", headers, values);
        _AetherRarity = ReadString("AetherRarity", headers, values);
    }
}
