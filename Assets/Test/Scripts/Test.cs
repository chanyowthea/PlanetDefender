using UnityEngine;
using System.Collections;
using System.Text;

public class Test : MonoBehaviour
{
    private void Awake()
    {
        SingletonManager.Init();
    }

    private void Start()
    {
        return;

        SingletonManager.SqliteHelper.DeleteTable(GameConfig.instance._AccountTableName); 
        SingletonManager.SqliteHelper.CreateTable(GameConfig.instance._AccountTableName,
            new string[] { "ID", "Name", "CurrentLevel" }, new string[] { "INTEGER PRIMARY KEY", "TEXT UNIQUE", "INTEGER" });
        int level = 4;

        //SingletonManager.SqliteHelper.InsertValues(GameConfig.instance._AccountTableName, 
        //    new string[] { "'2'", SingletonManager.SqliteHelper.GetStringForSQL(GameConfig.instance._AccountName + "0"), SingletonManager.SqliteHelper.GetStringForSQL(level.ToString()) });
        //SingletonManager.SqliteHelper.InsertValues(GameConfig.instance._AccountTableName, 
        //    new string[] { "'3'", SingletonManager.SqliteHelper.GetStringForSQL(GameConfig.instance._AccountName + "1"), SingletonManager.SqliteHelper.GetStringForSQL(level.ToString()) });

        //SingletonManager.SqliteHelper.GeRecordCount(GameConfig.instance._AccountTableName, 
        //    new string[] { "CurrentLevel" }, new string[] { "=" }, new string[] { SingletonManager.SqliteHelper.GetStringForSQL("4") }); 
        return; 

        //SingletonManager.SqliteHelper.InsertValues(GameConfig.instance._AccountTableName,
        //    new string[] { "'1'", SingletonManager.SqliteHelper.GetStringForSQL(GameConfig.instance._AccountName), SingletonManager.SqliteHelper.GetStringForSQL(level.ToString()) });

        //SingletonManager.SqliteHelper.UpdateValues(GameConfig.instance._AccountTableName, new string[] { "CurrenctLevel" }, new string[] { SingletonManager.SqliteHelper.GetStringForSQL(level.ToString()) }, "ID", "=", "'1'");
    }
}