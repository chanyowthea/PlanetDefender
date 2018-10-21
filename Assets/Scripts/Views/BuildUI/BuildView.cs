using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;

class BuildView : BaseUI
{
    [SerializeField] BuildSlot _SlotPrefab;
    [SerializeField] RectTransform _planetRtf;
    int _GapDegree = 60;
    List<BuildSlot> _Slots = new List<BuildSlot>();

    public BuildView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");

        _planetRtf.localEulerAngles = PlanetController.instance.transform.localEulerAngles;

        _SlotPrefab.gameObject.SetActive(false); 
        for (int i = 0, length = 360 / _GapDegree; i < length; i++)
        {
            int degree = i * _GapDegree;
            BuildSlot slot = GameObject.Instantiate(_SlotPrefab);
            slot.gameObject.SetActive(true); 
            slot.transform.SetParent(_planetRtf);
            slot.transform.localPosition = Vector3.zero;
            slot.transform.localScale = Vector3.one; 
            slot.transform.localEulerAngles = new Vector3(0, 0, degree);
            slot.SetData(degree, (d) => OnClickBuild(d));
            _Slots.Add(slot);
        }

        for (int i = 0, length = _Slots.Count; i < length; i++)
        {
            _Slots[i].OnBuild(0, false);
        }

        var cannons = TurretManager.instance.GetAllTurrets();
        for (int i = 0, length = cannons.Count; i < length; i++)
        {
            var c = cannons[i];
            if (c == null)
            {
                continue;
            }
            var cannon = c.GetComponentInChildren<Turret>();
            if (cannon == null)
            {
                continue;
            }

            // 是60的整数倍
            if (cannon._Degree % _GapDegree == 0)
            {
                var s = _Slots.Find((BuildSlot slot) => slot.Degree == cannon._Degree);
                if (s != null)
                {
                    s.OnBuild(cannon.TurretID, true);
                }
            }
        }

        var ui = UIManager.Instance.GetCurrentResidentUI<TopResidentUI>(); if (ui != null)
        {
            ui.UpdateView(true);
        }
    }

    internal override void Close()
    {
        for (int i = 0, length = _Slots.Count; i < length; i++)
        {
            var s = _Slots[i];
            s.ClearData();
            GameObject.Destroy(s.gameObject);
        }
        _Slots.Clear();

        EventDispatcher.instance.UnRegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
        base.Close();
    }

    void OnClickBuild(int degree)
    {
        var turret = TurretManager.instance.GetTurret(degree);
        if (turret != null)
        {
            return;
        }

        var view = UIManager.Instance.Open<TurretSelectView>();
        view.SetData(degree);
        Debug.Log("OnClickBuild index=" + degree);
    }

    void BuildSuccess(int degree, int turretId)
    {
        var s = _Slots.Find((BuildSlot slot) => slot.Degree == degree);
        if (s != null)
        {
            s.OnBuild(turretId, true);
        }
        UIManager.Instance.PopupBackTo<HUDView>();
        UIManager.Instance.Open<BuildView>();
    }
}
