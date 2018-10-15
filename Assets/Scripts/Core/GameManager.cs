using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UIFramework;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public DelayCallUtil _DelayCallUtil { private set; get; }
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
    UniqueIDGenerator _UniqueIDGenerator = new UniqueIDGenerator();
    Dictionary<uint, IEnumerator> _RoutineDict = new Dictionary<uint, IEnumerator>();

    private void Start()
    {
        _DelayCallUtil = gameObject.AddComponent<DelayCallUtil>();
        EventDispatcher.instance.RegisterEvent(EventID.End, this, "OnEnd");
        GameData.instance.Init();
        _DelayCallUtil.Timer._TimeScale = 0;
        ConfigDataManager.instance.LoadCSV<TurretCSV>("Turret");
        ConfigDataManager.instance.LoadCSV<LevelCSV>("Level");
        ConfigDataManager.instance.LoadCSV<OreCSV>("Ore");
        ConfigDataManager.instance.LoadCSV<EnemyCSV>("Enemy");
        ArchiveManager.instance.OnEnterPlay();
        PurchaseManager.instance.Init();
        int level = ArchiveManager.instance.GetCurrentLevel();
        var v = UIManager.Instance.Open<HUDView>();
        v.SetData(level);
        //for (int i = 1; i < 17; i++)
        //{
        //    ArchiveManager.instance.ChangeMaterialsCount(i, 100);
        //}
    }

    public void OnEnd()
    {
        _DelayCallUtil.Timer._TimeScale = 0;
        ArchiveManager.instance.OnQuitPlay();
        GameData.instance.Clear();
        TurretManager.instance.Clear();
        //CoroutineUtil.instance.ClearRoutines(ERoutinePlace.InGame);
    }

    public void AddHealth()
    {

    }

    private void OnDestroy()
    {
        PurchaseManager.instance.Clear();
        EventDispatcher.instance.UnRegisterEvent(EventID.End, this, "OnEnd");
    }

    private void OnApplicationQuit()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            EventDispatcher.instance.DispatchEvent(EventID.CreateTurret, 0, 1);
        }
        _DelayCallUtil.RunOneFrame();
    }

    void FixedUpdate()
    {
        _DelayCallUtil.FixedRunOneFrame();
    }

    public uint DelayCall(float delayTime, Action action, bool isRepeated = false)
    {
        return _DelayCallUtil.DelayCall(delayTime, action, isRepeated);
    }

    public void CancelDelayCall(uint id)
    {
        if (_DelayCallUtil == null)
        {
            return;
        }
        _DelayCallUtil.CancelDelayCall(id);
    }

    public uint CallEveryFrameInAPeriod(float time, Action<float> callEveryFrame, Action onFinish = null)
    {
        uint id = _UniqueIDGenerator.GetUniqueID();
        onFinish += () =>
        {
            if (_RoutineDict.ContainsKey(id))
            {
                StopCoroutine(_RoutineDict[id]);
                _RoutineDict.Remove(id);
            }
        };
        var routine = CallEveryFrameRoutine(time, callEveryFrame, onFinish);
        _RoutineDict.Add(id, routine);
        StartCoroutine(routine);
        return id;
    }

    public void CancelCallEveryFrameInAPeriod(uint id)
    {
        if (_RoutineDict.ContainsKey(id))
        {
            StopCoroutine(_RoutineDict[id]);
            _RoutineDict.Remove(id);
        }
    }

    IEnumerator CallEveryFrameRoutine(float expireTime, Action<float> callEveryFrame, Action onFinish)
    {
        float time = 0;
        while (time < expireTime)
        {
            yield return null;
            time += GameManager.instance._DelayCallUtil.Timer.DeltaTime;
            if (callEveryFrame != null)
            {
                callEveryFrame(time);
            }
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
}
