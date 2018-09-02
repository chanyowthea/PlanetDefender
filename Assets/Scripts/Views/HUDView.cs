using System.Collections;
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
    int _scoreCount; 
    int _CurLevel; 
    const string _scoreFormat = "Target Scores: {0}";

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
        EventDispatcher.instance.RegisterEvent(EventID.UpdateScore, this, "UpdateScore");
    }

    internal override void Show()
    {
        base.Show();
        Time.timeScale = 1; 
    }

    internal override void Close()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.UpdateScore, this, "UpdateScore");
        if (_addHealthRoutine != null)
        {
            StopCoroutine(_addHealthRoutine);
            _addHealthRoutine = null;
        }
        base.Close();
    }

    public void SetData(int level)
    {
        _CurLevel = level;
        UpdateView();
    }

    void UpdateView()
    {
        _scoreText.text = _scoreCount.ToString();
        LevelCSV csv = ConfigDataManager.instance.GetData<LevelCSV>(_CurLevel.ToString());
        if (csv != null)
        {
            _targetScoreText.text = string.Format(_scoreFormat, csv._TargetScore);
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

    public void OnClickBack()
    {
        EventDispatcher.instance.DispatchEvent(EventID.End);
        SceneManager.LoadScene(GameConfig.instance._LauncherSceneName); 
    }

    public void OnClickAttack()
    {
        if (PlanetController.instance.GetAllCannons().Length == 0)
        {
            var tips = UIManager.Instance.Open<MessageView>();
            tips.SetData("请建造炮塔后再进行攻击");
            return;
        }
        EventDispatcher.instance.DispatchEvent(EventID.AttackFromPlanet);
    }

    public void OnClickBuild()
    {
        UIManager.Instance.Open<BuildView>();
    }

    public void OnClickPause()
    {
        Time.timeScale = 0;
    }

    public void OnClickNormal()
    {
        Time.timeScale = 1;
    }

    public void OnClickAccelerate()
    {
        Time.timeScale = 2;
    }

    [SerializeField] Button _addHealthBtn;
    IEnumerator _addHealthRoutine;
    public void OnClickAddHealth()
    {
        if (!_addHealthBtn.enabled)
        {
            return;
        }
        if (_addHealthRoutine != null)
        {
            StopCoroutine(_addHealthRoutine);
            _addHealthRoutine = null;
        }
        _addHealthRoutine = AddHealthCountDown();
        StartCoroutine(_addHealthRoutine);

        // TODO 这个方式很烂，有没有好的方式？
        if (PlanetController.instance.IsHpLessThanMax())
        {
            if (GameData.instance.goldCount >= 1)
            {
                EventDispatcher.instance.DispatchEvent(EventID.AddHealth, 1);
                EventDispatcher.instance.DispatchEvent(EventID.AddGold, -1);
            }
            else
            {
                var v=  UIManager.Instance.Open<MessageView>(); 
                v.SetData("金币不足！"); 
            }
        }
    }

    IEnumerator AddHealthCountDown()
    {
        _addHealthBtn.enabled = false;
        yield return new WaitForSeconds(1);
        _addHealthBtn.enabled = true;
    }
}
