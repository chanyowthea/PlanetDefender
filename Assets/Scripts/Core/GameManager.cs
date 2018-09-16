using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UIFramework;
using UnityEngine;

// 名字: 保卫星球
// 炮塔可以升级和改变类型,升级后,攻击和防御力会有提升[读表,炮塔ID为Key,等级和]
// 季节变化
// 事件的随机出现,比如秒杀星球的超大陨石; 或者同时出现两个陨石,必须受伤

// 自由放置炮塔位置和炮塔类型,不要太多自动化
// 选择炮塔类型


// -- 增益
// 有金币道具,金币可以加血也可以升级炮塔
// 

// -- 限制
// 星球的旋转方向固定
// 随着时间的流逝,血量会自动减少



// -- 功能


// -- BUG
// 两个rock parent和 gold parent
// 没有出现rock


// 方案2 点击进入升级界面,拉近摄像机,显示可以装载的炮塔位置
// 排行榜


public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.End, this, "OnEnd");
        GameData.instance.Init();
        Time.timeScale = 0;
        ConfigDataManager.instance.LoadCSV<TurretCSV>("Turret");
        ConfigDataManager.instance.LoadCSV<LevelCSV>("Level");
        ConfigDataManager.instance.LoadCSV<OreCSV>("Ore");
        ArchiveManager.instance.OnEnterPlay(); 
        int level = ArchiveManager.instance.GetCurrentLevel();
        var v = UIManager.Instance.Open<HUDView>();
        v.SetData(level); 
    }

    public void OnEnd()
    {
        Time.timeScale = 0;
        ArchiveManager.instance.OnQuitPlay();
        GameData.instance.Clear();
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
    }
}
