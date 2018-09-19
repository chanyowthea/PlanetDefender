using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Reflection;

public class AccountData : SQLiteData
{
    public int _AccountID;
    public string _AccountName;
    public string _Golds;
    public string _HighestScores; 
    public string _Materials; 
}
