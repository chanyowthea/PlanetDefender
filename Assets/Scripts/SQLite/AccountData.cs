using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Reflection;

public class AccountData : SQLiteData
{
    public int _AccountID;
    public int _CurrentLevel; 
    public string _AccountName;
    public int _Golds;
    public int _HighestScores; 
    public string _Materials; 
}
