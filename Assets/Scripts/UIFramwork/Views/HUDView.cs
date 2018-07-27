using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HUDView : BaseView
{
    [SerializeField] Text _targetScoreText;
    [SerializeField] Text _scoreText;
    int _scoreCount;
    [SerializeField] Text _goldText;
    int _goldCount;
    const string _scoreFormat = "Target Scores: {0}";
    int _targetScore;

    private void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.UpdateGold, this, "UpdateGold");
        EventDispatcher.instance.RegisterEvent(EventID.UpdateScore, this, "UpdateScore");
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.UpdateScore, this, "UpdateScore");
        EventDispatcher.instance.UnRegisterEvent(EventID.UpdateGold, this, "UpdateGold");
    }

    public override void Open()
    {
        base.Open();
        UpdateGold(GameData.instance.goldCount);
    }

    public override void Close()
    {
        if (_addHealthRoutine != null)
        {
            StopCoroutine(_addHealthRoutine);
            _addHealthRoutine = null;
        }
        base.Close();
    }

    public void SetData(int targetScore)
    {
        _targetScore = targetScore;
        UpdateView();
    }

    void UpdateView()
    {
        _goldText.text = _goldCount.ToString();
        _scoreText.text = _scoreCount.ToString();
        _targetScoreText.text = string.Format(_scoreFormat, _targetScore);
    }

    public void UpdateGold(int value)
    {
        _goldCount = value;
        UpdateView();
    }

    public void UpdateScore(int value)
    {
        _scoreCount = value;
        if (_scoreCount >= _targetScore)
        {
            EventDispatcher.instance.DispatchEvent(EventID.End);
            EventDispatcher.instance.DispatchEvent(EventID.AddGold, _targetScore);
            var v = ViewManager.instance.Open<EndView>();
            v.SetData(string.Format("Mission Accomplished! \nGet {0} Golds! ", _targetScore));
        }
        UpdateView();
    }

    public void OnClickBack()
    {
        EventDispatcher.instance.DispatchEvent(EventID.End);
//        ViewManager.instance.Close(this.GetHashCode());
		ViewManager.instance.Open<StartView>(true); 
    }

    public void OnClickAttack()
    {
        if (PlanetController.instance.GetAllCannons().Length == 0)
        {
            var tips = ViewManager.instance.Open<TipsView>();
            tips.SetData("请建造炮塔后再进行攻击");
            return;
        }
        EventDispatcher.instance.DispatchEvent(EventID.AttackFromPlanet);
    }

    public void OnClickBuild()
    {
        ViewManager.instance.Open<BuildView>();
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
                var v=  ViewManager.instance.Open<TipsView>(); 
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
