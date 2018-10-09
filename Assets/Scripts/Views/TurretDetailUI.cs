using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class TurretDetailUI : BaseUI
{
    [SerializeField] Text _NameText;
    [SerializeField] Text _AttackText;
    [SerializeField] Text _DefenseText;
    [SerializeField] Text _PriceText;
    [SerializeField] CustomImage _Picture; 
    Action _OnPurchase;
    int _TurretId; 

    public TurretDetailUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
    }

    public override void Open(NavigationData data = null)
    {
        base.Open(data);
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
    }

    internal override void Close()
    {
        ResourcesManager.instance.UnloadAsset(_Picture.sprite);
        _Picture.sprite = null;
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
        base.Close();
    }

    void BuildSuccess(int degree, int turretId)
    {
        UIManager.Instance.Close(this);
    }

    public void OnClickClose()
    {
        UIManager.Instance.Close(this);
    }

    public void OnClickPurchase()
    {
        if (_OnPurchase != null)
        {
            _OnPurchase();
        }
    }

    public void SetData(int turretId, Action onPurchase)
    {
        _TurretId = turretId;
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(turretId.ToString());
        if (csv == null)
        {
            Debugger.Log("cannot find csv data with id " + turretId); 
            return;
        }
        _NameText.text = csv._Name;
        _AttackText.text = csv._Attack.ToString();
        _DefenseText.text = csv._Defense.ToString(); 
        _PriceText.text = csv._Price.ToString(); 
        _Picture.SetData(ResourcesManager.instance.GetSprite(csv._Picture));
        _OnPurchase = onPurchase;
    }
}
