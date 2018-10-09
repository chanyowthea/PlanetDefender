using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UIFramework;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public readonly TimeService _Timer = new TimeService(); 
    
    private void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.End, this, "OnEnd");
        GameData.instance.Init();
        _Timer._TimeScale = 0;
        ConfigDataManager.instance.LoadCSV<TurretCSV>("Turret");
        ConfigDataManager.instance.LoadCSV<LevelCSV>("Level");
        ConfigDataManager.instance.LoadCSV<OreCSV>("Ore");
        ConfigDataManager.instance.LoadCSV<EnemyCSV>("Enemy");
        ArchiveManager.instance.OnEnterPlay(); 
        int level = ArchiveManager.instance.GetCurrentLevel();
        var v = UIManager.Instance.Open<HUDView>();
        v.SetData(level); 
    }

    public void OnEnd()
    {
        _Timer._TimeScale = 0;
        ArchiveManager.instance.OnQuitPlay();
        GameData.instance.Clear();
        TurretManager.instance.Clear();
        CoroutineUtil.instance.ClearRoutines(ERoutinePlace.InGame); 
    }

    public void AddHealth()
    {
    }

    private void OnDestroy()
    {
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
        _Timer.UpdateTime(); 
    }
}
