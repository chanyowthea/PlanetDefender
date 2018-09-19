using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        var data = new AccountData();
        var ts = data.ToSqliteTypes(); 
        DebugFramework.Debugger.Log(ts.GetLogString()); 

        Facade.instance.Init(); 
        UIManager.Instance.Open<StartView>();
    }
}
