﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

class HUDView : BaseUI
{
    [SerializeField] Text _targetScoreText;
    [SerializeField] Text _scoreText;
    [SerializeField] Toggle _PauseToggle;
    [SerializeField] Toggle _NormalToggle;
    [SerializeField] Toggle _AccelerateToggle;
    int _scoreCount;
    int _CurLevel;
    CameraInputUI _CameraInput;
    bool _IsShowMiniMap;

    public HUDView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = false;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        UIManager.Instance.Open<TopResidentUI>();
        var ui = UIManager.Instance.GetCurrentResidentUI<TopResidentUI>(); if (ui != null)
        {
            ui.UpdateView(true, false);
        }
        EventDispatcher.instance.RegisterEvent(EventID.UpdateScore, this, "UpdateScore");
        _HealImage.material.SetFloat("_Ratio", 0);
        _PauseToggle.onValueChanged.AddListener(OnClickPause);
        _NormalToggle.onValueChanged.AddListener(OnClickNormal);
        _AccelerateToggle.onValueChanged.AddListener(OnClickAccelerate);
        //_CameraInput = UIManager.Instance.Open<CameraInputUI>();
    }

    internal override void Show()
    {
        base.Show();
        Debugger.Log(LogUtil.GetCurMethodName());
        GameManager.instance.RestoreTimeScale();
        var ui = UIManager.Instance.GetCurrentResidentUI<TopResidentUI>(); if (ui != null)
        {
            ui.UpdateView(true, false);
        }
        if (_CameraInput != null)
        {
            _CameraInput.Show();
        }
    }

    internal override void Hide()
    {
        if (_CameraInput != null)
        {
            _CameraInput.Hide();
        }
        GameManager.instance.TimeScale = 0;
        base.Hide();
        Debugger.Log(LogUtil.GetCurMethodName());
    }

    internal override void Close()
    {
        //UIManager.Instance.Close<CameraInputUI>();
        _PauseToggle.onValueChanged.RemoveListener(OnClickPause);
        _NormalToggle.onValueChanged.RemoveListener(OnClickPause);
        _AccelerateToggle.onValueChanged.RemoveListener(OnClickAccelerate);
        if (_DelayCallID != 0)
        {
            GameManager.instance.CancelCallEveryFrameInAPeriod(_DelayCallID);
            _DelayCallID = 0;
        }

        EventDispatcher.instance.UnRegisterEvent(EventID.UpdateScore, this, "UpdateScore");

        if (_DelayCallID != 0)
        {
            Facade.instance.CancelDelayCall(_DelayCallID);
            _DelayCallID = 0;
        }
        base.Close();
    }

    public void SetData(int level)
    {
        _HealImage.material = null;
        _HealImage.material = GameObject.Instantiate(GameAssets.instance._RatioRectMaterial);
        _CurLevel = level;
        UpdateView();
    }

    internal override void ClearData()
    {
        _IsShowMiniMap = false; 
        if (_HealImage.material != null)
        {
            GameObject.Destroy(_HealImage.material);
        }
        _scoreCount = 0;
        _CurLevel = 0;
        base.ClearData();
    }

    void UpdateView()
    {
        _scoreText.text = _scoreCount.ToString();
        LevelCSV csv = ConfigDataManager.instance.GetData<LevelCSV>(_CurLevel.ToString());
        if (csv != null)
        {
            _targetScoreText.text = csv._TargetScore.ToString();
        }
    }

    public void UpdateScore(int value)
    {
        _scoreCount = value;
        LevelCSV csv = ConfigDataManager.instance.GetData<LevelCSV>(_CurLevel.ToString());
        if (csv == null)
        {
            Debug.LogError(string.Format("cannot find csv data with id {0}. ", _CurLevel));
            return;
        }

        if (_scoreCount >= csv._TargetScore)
        {
            EventDispatcher.instance.DispatchEvent(EventID.End);
            EventDispatcher.instance.DispatchEvent(EventID.AddGold, _CurLevel);
            var v = UIManager.Instance.Open<EndView>();
            v.SetData(string.Format("Mission Accomplished! \nGet {0} Golds! ", _CurLevel));
        }
        UpdateView();
    }

    public void OnClickSettings()
    {
        UIManager.Instance.Open<SettingsUI>();
    }

    public void OnClickAttack()
    {
        if (TurretManager.instance.GetAllTurrets().Count == 0)
        {
            MessageManager.instance.ShowTips("请建造炮塔后再进行攻击");
            return;
        }
        EventDispatcher.instance.DispatchEvent(EventID.AttackFromPlanet);
    }

    public void OnClickPause(bool value)
    {
        if (value)
        {
            GameManager.instance.TimeScale = 0;
        }
    }

    public void OnClickNormal(bool value)
    {
        if (value)
        {
            GameManager.instance.TimeScale = 1;
        }
    }

    public void OnClickAccelerate(bool value)
    {
        if (value)
        {
            GameManager.instance.TimeScale = 2;
        }
    }

    public void OnClickMiniMap()
    {
        if (!_IsShowMiniMap)
        {
            UIManager.Instance.Open<MiniMapUI>();
        }
        else
        {
            UIManager.Instance.Close<MiniMapUI>();
        }
        _IsShowMiniMap = !_IsShowMiniMap; 
    }

    public void OnClickBuild()
    {
        UIManager.Instance.Open<BuildView>();
    }
    [SerializeField] Image _HealImage;
    uint _DelayCallID;
    public void OnClickAddHealth()
    {
        if (_DelayCallID != 0)
        {
            return;
        }
        if (PlanetController.instance.IsHpLessThanMax())
        {
            if (ArchiveManager.instance.GetGoldCount() >= 1)
            {
                EventDispatcher.instance.DispatchEvent(EventID.AddHealth, 1);
                EventDispatcher.instance.DispatchEvent(EventID.AddGold, -1);

                float maxTime = 1;
                _DelayCallID = GameManager.instance.CallEveryFrameInAPeriod(maxTime, (time) =>
                {
                    _HealImage.material.SetFloat("_Ratio", (maxTime - time) / maxTime);
                }, () => _DelayCallID = 0);
            }
            else
            {
                MessageManager.instance.ShowTips("金币不足！");
            }
        }
    }

#if UNITY_EDITOR || UNITY_EDITOR_WIN
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnClickAttack();
        }
    }
#endif
}
