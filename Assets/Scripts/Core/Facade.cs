using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using System;

public class Facade : MonoSingleton<Facade>
{
    DelayCallUtil _DelayCallUtil;
    bool _HasInitialized;
    public float TimeScale
    {
        get
        {
            return _DelayCallUtil.Timer._TimeScale;
        }
        set
        {
            _DelayCallUtil.Timer._TimeScale = value;
        }
    }

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
        _DelayCallUtil = gameObject.AddComponent<DelayCallUtil>();
        MessageManager.instance.Init(); 
    }

    private void OnDestroy()
    {
        MessageManager.instance.Clear();
    }

    public void ChangeScene(string sceneName)
    {
        UIManager.Instance.ChangeScene();
        var load = UIManager.Instance.Open<LoadingView>();
        load.SetData(sceneName);
    }

    public uint DelayCall(float delayTime, Action action, bool isRepeated = false)
    {
        return _DelayCallUtil.DelayCall(delayTime, action, isRepeated);
    }

    public void CancelDelayCall(uint id)
    {
        _DelayCallUtil.CancelDelayCall(id);
    }

    private void Update()
    {
        _DelayCallUtil.RunOneFrame();
    }

    void FixedUpdate()
    {
        _DelayCallUtil.FixedRunOneFrame();
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

