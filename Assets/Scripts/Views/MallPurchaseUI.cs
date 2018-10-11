using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class MallPurchaseUI : BaseUI
{
    int _Number = 1;
    int Number
    {
        get
        {
            int.TryParse(_NumberInput.text, out _Number);
            return _Number;
        }
        set
        {
            _Number = value;
            _NumberInput.text = value.ToString();
        }
    }

    [SerializeField] Text _MessageText;
    [SerializeField] InputField _NumberInput;
    Action _OnOK;
    int _ID;
    int _MaxNumber = 1;

    public MallPurchaseUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
    }

    public void OnClickClose()
    {
        UIManager.Instance.Close(this);
    }

    public void OnClickOK()
    {
        if (Number > _MaxNumber)
        {
            var ui = UIManager.Instance.Open<MessageView>();
            ui.SetData("Out of stock! "); 
            Debugger.LogError("out of stock! ");
            Number = _MaxNumber;
            return; 
        }

        var code = PurchaseManager.instance.BuyItem(_ID, Number);
        if (code == 0)
        {
            if (_OnOK != null)
            {
                _OnOK();
            }
            UIManager.Instance.Close(this);
        }
    }

    public void SetData(string msg, Action onOK, int id, int maxNumber)
    {
        _MessageText.text = msg;
        _OnOK = onOK;
        _MaxNumber = maxNumber;
        _ID = id;
        Number = 1;
    }
}
