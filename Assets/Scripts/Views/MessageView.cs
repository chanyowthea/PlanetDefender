using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class MessageView : BaseUI
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Text _messageText;
    uint _DelayCallID;

    public MessageView()
    {
        _NaviData._Type = EUIType.Independent;
        _NaviData._Layer = EUILayer.Tips;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        Facade.instance.DelayCall(1, () =>
        {
            UIManager.Instance.Close(this);
            _DelayCallID = 0;
        });
    }

    internal override void Close()
    {
        if (_DelayCallID != 0)
        {
            Facade.instance.CancelDelayCall(_DelayCallID);
            _DelayCallID = 0;
        }
        base.Close();
    }

    public void SetData(string s)
    {
        _messageText.text = s;
    }
}