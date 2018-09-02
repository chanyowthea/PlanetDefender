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
        _routine = Routine(); 
        //CoroutineUtil.instance.StartCoroutine(_routine);
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

    IEnumerator Routine()
    {
        _canvasGroup.alpha = 1;
        yield return new WaitForSecondsRealtime(1);
        float time = 0;
        while (time <= 1)
        {
            yield return null;
            time += Time.deltaTime;
            _canvasGroup.alpha = 1 - time / 1;
        }
        UIManager.Instance.Close(this);
    }
}