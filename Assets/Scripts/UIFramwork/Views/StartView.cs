using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;

public class StartView : BaseView
{
    public void OnClickStart(int level)
    {
        Time.timeScale = 1;
        var v = ViewManager.instance.Open<HUDView>();
        v.SetData(5 * level * level); 
    }
}
