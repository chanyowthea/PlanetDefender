using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class MessageView : BaseUI
{
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Text _messageText;
    IEnumerator _routine;

    public MessageView()
    {
        _NaviData._Type = EUIType.Independent;
        _NaviData._Layer = EUILayer.Tips;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        CoroutineUtil.instance.Wait(1, () => UIManager.Instance.Close(this), true);
    }

    internal override void Close()
    {
        if (_routine != null)
        {
            CoroutineUtil.instance.StopCoroutine(_routine);
            _routine = null;
        }
        base.Close();
    }

    public void SetData(string s)
    {
        _messageText.text = s;
    }
}