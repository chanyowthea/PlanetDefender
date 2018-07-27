using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : TSingleton<ArchiveManager>
{
    const string _goldCountKey = "goldCount";
    //const string _hpKey = "hp";
    public int GetGoldCount()
    {
        // 设置初始金币值
        if (!PlayerPrefs.HasKey(_goldCountKey))
        {
            PlayerPrefs.SetInt(_goldCountKey, 20);
        }
        return PlayerPrefs.GetInt(_goldCountKey);
    }

    //public int GetHP()
    //{
    //    // 设置初始金币值
    //    if (!PlayerPrefs.HasKey(_hpKey))
    //    {
    //        PlayerPrefs.SetInt(_hpKey, 20);
    //    }
    //    return PlayerPrefs.GetInt(_hpKey);
    //}

    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll(); 
    }

    public void SaveAllData()
    {
        PlayerPrefs.SetInt(_goldCountKey, GameData.instance.goldCount);
        //PlayerPrefs.SetInt(_hpKey, PlanetController.instance.GetHP());
    }
}
