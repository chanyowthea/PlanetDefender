using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class Facade : MonoSingleton<Facade>
{
    public readonly TimeService _UITimer = new TimeService();
    public TimeService CurTimer
    {
        get
        {
            return CurRoutinePlace == ERoutinePlace.InGame ? GameManager.instance._Timer : _UITimer;
        }
    }
    public ERoutinePlace CurRoutinePlace { private set; get; }

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
        load.SetData(sceneName, (sName) =>
        {
            //if (sName == GameConfig.instance._PlaySceneName)
            //{
            //    CurRoutinePlace = ERoutinePlace.InGame;
            //}
            //else
            //{
            //    CurRoutinePlace = ERoutinePlace.UI;
            //}
        });
    }

    private void Update()
    {
        _UITimer.UpdateTime(); 
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

