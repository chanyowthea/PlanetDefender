using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

class StartView : BaseUI
{
    public StartView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }
    
    public override void Open(NavigationData data = null)
    {
        base.Open(data);
        var csv = ConfigDataManager.instance.GetData<TurrectCSV>("2");
        Debug.Log("data.Name=" + csv._Name);
    }

    internal override void Show()
    {
        base.Show();
        UIManager.Instance.Close<TopResidentUI>(); 
    }

    public void OnClickStart(int level)
    {
        Time.timeScale = 1;
        var v = UIManager.Instance.Open<HUDView>();
        v.SetData(5 * level * level); 
    }
}
