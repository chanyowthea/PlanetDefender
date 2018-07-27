using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;
using UnityEngine.UI; 

public class BuildView : BaseView
{
    [SerializeField] Button[] _btns;
    [SerializeField] Image[] _imgs;
    [SerializeField] Text _goldText;
    [SerializeField] RectTransform _planetRtf;
    int _goldCount;
	float _lastTimeScale; 

    private void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.UpdateGold, this, "UpdateGold");
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.UpdateGold, this, "UpdateGold");
        EventDispatcher.instance.UnRegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
    }

    void UpdateView()
    {
        _goldText.text = _goldCount.ToString();
    }

    public void UpdateGold(int value)
    {
        _goldCount = value;
        UpdateView();
    }

    public override void Open()
    {
        base.Open();
        _planetRtf.localEulerAngles = PlanetController.instance.transform.localEulerAngles;
		_lastTimeScale = Time.timeScale; 
        Time.timeScale = 0; 

        if (_btns.Length != _imgs.Length)
        {
            Debug.LogError("the length of btns and imgs is not corresponding! please check your configuration first! ");
            return; 
        }

        for (int i = 0, length = _btns.Length; i < length; i++)
        {
            OnBuild(i, false); 
        }

        var cannons = PlanetController.instance.GetAllCannons();
        for (int i = 0, length = cannons.Length; i < length; i++)
        {
            var c = cannons[i];
            if (c == null)
            {
                continue; 
            }
            var cannon = c.GetComponentInChildren<Cannon>();
            if (cannon == null)
            {
                continue; 
            }

            // 是60的整数倍
            if (cannon._degree % 60 == 0)
            {
                int index = cannon._degree / 60;
                if (index >= 0 && index < _btns.Length)
                {
                    OnBuild(index); 
                }
            }
        }

        for (int i = 0, length = _btns.Length; i < length; i++)
        {
            int index = i; 
            _btns[i].onClick.AddListener(() => OnClickBuild(index)); 
        }

        UpdateGold(GameData.instance.goldCount); 
    }

    public override void Close()
    {
		Time.timeScale = _lastTimeScale;
        for (int i = 0, length = _btns.Length; i < length; i++)
        {
            _btns[i].onClick.RemoveAllListeners();
        }
        base.Close();
    }
     
    public void OnClickBack()
    {
        ViewManager.instance.Close(GetHashCode()); 
    }

    void OnClickBuild(int index)
    {
        var cs = PlanetController.instance.GetAllCannons();
        if (cs != null)
        {
            for (int i = 0, length = cs.Length; i < length; i++)
            {
                var c = cs[i];
                // 已经建造了不可再建造
                if (c._degree == index * 60)
                {
                    return; 
                }
            }
        }

        EventDispatcher.instance.DispatchEvent(EventID.CreateTurret, index * 60);
    }

    void BuildSuccess(int degree)
    {
        OnBuild(degree / 60); 
    }

    void OnBuild(int index, bool isBuild = true)
    {
        _btns[index].gameObject.SetActive(!isBuild);
        _imgs[index].gameObject.SetActive(isBuild);
    }
}
