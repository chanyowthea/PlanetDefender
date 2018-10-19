using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class MessageView : BaseUI
{
    //[SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] Text _messageText;
    //uint _DelayCallID;

    public MessageView()
    {
        _NaviData._Type = EUIType.Independent;
        _NaviData._Layer = EUILayer.Tips;
    }

    //public override void Open(NavigationData data)
    //{
    //    base.Open(data);
    //    //Facade.instance.DelayCall(1, () =>
    //    //{
    //    //    UIManager.Instance.Close(this);
    //    //    _DelayCallID = 0;
    //    //});
    //}

    //internal override void Close()
    //{
    //    if (_DelayCallID != 0)
    //    {
    //        Facade.instance.CancelDelayCall(_DelayCallID);
    //        _DelayCallID = 0;
    //    }
    //    base.Close();
    //}

    //public void SetData(string s)
    //{
    //    _messageText.text = s;
    //}

    private Vector3 m_BasePos;
    private Vector3 m_CurrentPos;
    private Vector3 m_Offset;

    private float m_ClipLength = 3;
    [SerializeField] TweenPosition m_ParentTP;
    [SerializeField] RectTransform _BG; 
    public void ShowTips(string tips)
    {
        Show(); 
        m_Offset = new Vector3(0, _BG.rect.height + 4, 0);
        m_BasePos = m_ParentTP.transform.localPosition;
        m_CurrentPos = m_BasePos;
        _messageText.text = tips;
        StartCoroutine("OnFinished");
    }

    public void MoveUp()
    {
        m_ParentTP.from = m_CurrentPos;
        m_ParentTP.to = m_CurrentPos += m_Offset;
        m_ParentTP.duration = 0.3f;
        m_ParentTP.PlayForward();
    }

    public IEnumerator OnFinished()
    {
        yield return new WaitForSeconds(m_ClipLength + 0.3f);
        m_ParentTP.ResetToBeginning();
        m_ParentTP.transform.localPosition = m_BasePos;
        m_CurrentPos = m_BasePos;
        MessageManager.instance.UpdateRunningIndex();
        Hide(); 
    }
}