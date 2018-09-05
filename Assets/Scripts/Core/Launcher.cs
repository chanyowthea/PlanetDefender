using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        SingletonManager.Init();
        SingletonManager.SqliteHelper.DeleteTable(GameConfig.instance._AccountTableName);
        SingletonManager.SqliteHelper.CreateTable(GameConfig.instance._AccountTableName, new string[] { "ID", "Name", "CurrentLevel" }, 
            new string[] { "INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT UNIQUE", "INTEGER" });
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName, 
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName), 
            new Mono.Data.Sqlite.SqliteParameter("CurrentLevel", "0"));
        ConfigDataManager.instance.LoadCSV<UICSV>("UI");
        UIManager.Instance.Open<StartView>();
    }
}
