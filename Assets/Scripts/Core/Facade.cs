using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class Facade : MonoSingleton<Facade>
{
    bool _HasInitialized;
    public void Init()
    {
        if (_HasInitialized)
        {
            return;
        }
        _HasInitialized = true;

        DontDestroyOnLoad(this);
        gameObject.name = GetType().ToString();
        if (Debugger.LogToFile == true)
        {
            gameObject.AddComponent<DebuggerFileOutput>();
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        SingletonManager.Init();
        ArchiveManager.instance.Init();
        ConfigDataManager.instance.LoadCSV<LocCSV>("Loc");
        ConfigDataManager.instance.LoadCSV<UICSV>("UI");
        LocManager.instance.Init(LocLang.English);
    }

    public void ChangeScene(string sceneName)
    {
        UIManager.Instance.ChangeScene();
        var load = UIManager.Instance.Open<LoadingView>();
        load.SetData(sceneName);
    }

    void OnApplicationQuit()
    {
        Debugger.OnApplicationQuit();
    }

    void OnApplicationPause(bool paused)
    {
        if (paused)
        {
            Debugger.Flush();
        }
    }
}

