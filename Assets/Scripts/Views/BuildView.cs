using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;

class BuildView : BaseUI
{
    [SerializeField] Button[] _btns;
    [SerializeField] Image[] _imgs;
    [SerializeField] RectTransform _planetRtf;
    float _lastTimeScale;

    public BuildView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    void UpdateView()
    {

    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");

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
            OnBuild(i, 0, false);
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
            if (cannon._Degree % 60 == 0)
            {
                int index = cannon._Degree / 60;
                if (index >= 0 && index < _btns.Length)
                {
                    OnBuild(index, cannon.TurrectID);
                }
            }
        }

        for (int i = 0, length = _btns.Length; i < length; i++)
        {
            int index = i;
            _btns[i].onClick.AddListener(() => OnClickBuild(index));
        }
    }

    internal override void Close()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
        Time.timeScale = _lastTimeScale;
        for (int i = 0, length = _btns.Length; i < length; i++)
        {
            _btns[i].onClick.RemoveAllListeners();
        }
        base.Close();
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
                if (c._Degree == index * 60)
                {
                    return;
                }
            }
        }

        var view = UIManager.Instance.Open<TurretSelectView>();
        view.SetData(index * 60); 
        //EventDispatcher.instance.DispatchEvent(EventID.CreateTurret, index * 60);
        Debug.Log("OnClickBuild index=" + index);
    }

    void BuildSuccess(int degree, int turretId)
    {
        OnBuild(degree / 60, turretId);
    }

    void OnBuild(int index, int turrectId, bool isBuild = true)
    {
        _btns[index].gameObject.SetActive(!isBuild);
        var image = _imgs[index];
        image.gameObject.SetActive(isBuild);
        if (turrectId != 0)
        {
            var csv = ConfigDataManager.instance.GetData<TurretCSV>(turrectId.ToString());
            if (csv == null)
            {
                Debug.LogError("CreateCannon csv is empty! ");
                return;
            }
            image.sprite = ResourcesManager.instance.GetSprite(csv._Picture); 
        }
    }
}
