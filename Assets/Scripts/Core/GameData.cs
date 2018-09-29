using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 场景中的数据
public class GameData : TSingleton<GameData>
{
    public int TargetScore { private set; get; }
    public int scoreCount{private set; get; }

    public override void Init()
    {
        base.Init();
        EventDispatcher.instance.RegisterEvent(EventID.AddScore, this, "AddScore");
    }

    public override void Clear()
    {
        scoreCount = 0; 
        EventDispatcher.instance.UnRegisterEvent(EventID.AddScore, this, "AddScore");
        base.Clear();
    }

    public void SetTargetScore(int score)
    {
        TargetScore = score;
    }

    void AddScore(int value)
    {
        scoreCount += value;
        Debugger.Log("AddScore scoreCount=" + scoreCount); 
        EventDispatcher.instance.DispatchEvent(EventID.UpdateScore, scoreCount);
    }
}
