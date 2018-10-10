using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;

public class Test : MonoBehaviour
{
    void Start()
    {
        Debugger.LogGreen("666");
        Debugger.LogFormat("666", 1);
        Debugger.LogFormat("666 {0}", 1);
        Debugger.LogErrorFormat("666", 1);
        Debugger.LogErrorFormat("666 {0}", 1);
    }
}