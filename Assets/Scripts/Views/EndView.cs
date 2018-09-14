using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class EndView : BaseUI
{
    [SerializeField] Text _resultText;

    public EndView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public override void Open(NavigationData data = null)
    {
        base.Open(data);
        UIManager.Instance.Close<TopResidentUI>();
    }

    public void OnClickEnd()
    {
        Facade.instance.ChangeScene(GameConfig.instance._LauncherSceneName); 
    }

    public void SetData(string result)
    {
        _resultText.text = result;
    }
}
