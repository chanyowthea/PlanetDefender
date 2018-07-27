using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : TSingleton<GameData>
{
    public int goldCount { private set; get; }
    public int scoreCount { private set; get; }

    public override void Init()
    {
        base.Init();
        EventDispatcher.instance.RegisterEvent(EventID.AddGold, this, "AddGold");
        EventDispatcher.instance.RegisterEvent(EventID.AddScore, this, "AddScore");
        goldCount = ArchiveManager.instance.GetGoldCount();
        scoreCount = 0; 
    }

    public override void Clear()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.AddScore, this, "AddScore");
        EventDispatcher.instance.UnRegisterEvent(EventID.AddGold, this, "AddGold");
        base.Clear();
    }

    void AddGold(int value)
    {
        goldCount += value;
        EventDispatcher.instance.DispatchEvent(EventID.UpdateGold, goldCount);
    }

    void AddScore(int value)
    {
        scoreCount += value;
        EventDispatcher.instance.DispatchEvent(EventID.UpdateScore, scoreCount);
    }
}
