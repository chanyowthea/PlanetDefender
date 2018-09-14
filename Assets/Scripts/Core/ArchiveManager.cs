using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : TSingleton<ArchiveManager>
{
    public override void Init()
    {
        base.Init();

        // delete the table to recreate it. 
        //SingletonManager.SqliteHelper.DeleteTable(GameConfig.instance._AccountTableName);

        // if the table not exist, create it. 
        SingletonManager.SqliteHelper.CreateTable(GameConfig.instance._AccountTableName, new string[] 
            { "ID", "Name", "CurrentLevel", "Golds", "HighestScores" },
            new string[] 
            { "INTEGER PRIMARY KEY AUTOINCREMENT", "TEXT UNIQUE", "INTEGER", "INTEGER", "INTEGER" });

        // if the account information not exist, create it. 
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter("CurrentLevel", "0"),
            new Mono.Data.Sqlite.SqliteParameter("Golds", "10000"),
            new Mono.Data.Sqlite.SqliteParameter("HighestScores", "0"));
    }

    public void OnEnterPlay()
    {
        EventDispatcher.instance.RegisterEvent(EventID.AddGold, this, "AddGold");
    }

    public void OnQuitPlay()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.AddGold, this, "AddGold");
    }

    void AddGold(int value)
    {
        int golds = GetGoldCount(); 
        golds += value;
        SetGoldCount(golds); 
        EventDispatcher.instance.DispatchEvent(EventID.UpdateGold, GetGoldCount());
    }

    public override void Clear()
    {
        base.Clear();
    }

    public int GetGoldCount()
    {
        int value = 0;
        var reader = SingletonManager.SqliteHelper.ReaderInfo(GameConfig.instance._AccountTableName, new string[] { "Golds" },
            new Mono.Data.Sqlite.SqliteParameter("@Name", GameConfig.instance._AccountName));
        if (reader.Count > 0 && reader[0].Count > 0)
        {
            int.TryParse(reader[0][0].ToString(), out value);
        }
        return value;
    }

    public void SetGoldCount(int value)
    {
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter("Golds", value));
    }

    public void DeleteAllData()
    {

    }

    public void SaveAllData()
    {

    }


    public int GetCurrentLevel()
    {
        int level = 0;
        var reader = SingletonManager.SqliteHelper.ReaderInfo(GameConfig.instance._AccountTableName, new string[] { "CurrentLevel" },
            new Mono.Data.Sqlite.SqliteParameter("@Name", GameConfig.instance._AccountName));
        if (reader.Count > 0 && reader[0].Count > 0)
        {
            int.TryParse(reader[0][0].ToString(), out level);
        }
        return level;
    }

    public void SetCurrentLevel(int level)
    {
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
            new Mono.Data.Sqlite.SqliteParameter("Name", GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter("CurrentLevel", level));
    }
}
