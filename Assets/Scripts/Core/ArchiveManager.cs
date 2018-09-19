using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchiveManager : TSingleton<ArchiveManager>
{
    public AccountData AccountInfo { private set; get; }
    public override void Init()
    {
        base.Init();

        // delete the table to recreate it. 
        //SingletonManager.SqliteHelper.DeleteTable(GameConfig.instance._AccountTableName);

        // if the table do not exist, create it. 
        AccountInfo = new AccountData();
        Dictionary<string, ESQLiteAttributeType> dict = new Dictionary<string, ESQLiteAttributeType>();
        dict.Add(LogUtil.GetVarName(rt => AccountInfo._AccountID), ESQLiteAttributeType.PrimaryKeyAutoIncrement);
        dict.Add(LogUtil.GetVarName(rt => AccountInfo._AccountName), ESQLiteAttributeType.Unique);
        SingletonManager.SqliteHelper.CreateTable(GameConfig.instance._AccountTableName, AccountInfo.ToFieldNames(), AccountInfo.ToSqliteTypes(dict));

        // if the account information do not exist, create it. 
        if (SingletonManager.SqliteHelper.GetCount(GameConfig.instance._AccountTableName,
            new SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._AccountName), GameConfig.instance._AccountName)) == 0)
        {
            AccountInfo._AccountName = GameConfig.instance._AccountName;
            AccountInfo._CurrentLevel = 0;
            AccountInfo._Golds = 10000;
            AccountInfo._HighestScores = 0;
            Dictionary<int, int> ms = new Dictionary<int, int>();
            ms.Add(1, 10);
            ms.Add(2, 10);
            ms.Add(3, 10);
            AccountInfo._Materials = ms.GetLogString();

            SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
                new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._AccountName), GameConfig.instance._AccountName), 
                AccountInfo.ToSqliteParams());
        }
        UpdateAccountInfo();
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
        return AccountInfo._Golds;
    }

    public void SetGoldCount(int value)
    {
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
            new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._AccountName), GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._Golds), value));
        AccountInfo._Golds = value;
    }

    public int GetCurrentLevel()
    {
        return AccountInfo._CurrentLevel;
    }

    public void SetCurrentLevel(int level)
    {
        SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName,
            new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._AccountName), GameConfig.instance._AccountName),
            new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._CurrentLevel), level));
        AccountInfo._CurrentLevel = level;
    }

    public void UpdateAccountInfo()
    {
        var reader = SingletonManager.SqliteHelper.ReaderInfo(GameConfig.instance._AccountTableName,
            AccountInfo.ToFieldNames(),
            new Mono.Data.Sqlite.SqliteParameter(LogUtil.GetVarName(rt => AccountInfo._AccountName), GameConfig.instance._AccountName));
        if (reader.Count > 0 && reader[0].Count > 0)
        {
            AccountInfo.SetAllValues(reader[0]);
        }
    }
}
