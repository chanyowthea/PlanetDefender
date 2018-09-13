using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class PromptView : BaseUI
{
    [SerializeField] Text _MessageText;
    Action _OnOK;
    Action _OnCancel;

    public PromptView()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
    }

    public void OnClickCancel()
    {
        if (_OnCancel != null)
        {
            _OnCancel();
        }
        UIManager.Instance.Close(this);
    }

    public void OnClickOK()
    {
        if (_OnOK != null)
        {
            _OnOK();
        }
        UIManager.Instance.Close(this);
    }

    public void SetData(string msg, Action onOK, Action onCancel = null)
    {
        _MessageText.text = msg;
        _OnOK = onOK;
        _OnCancel = onCancel; 
    }
}
