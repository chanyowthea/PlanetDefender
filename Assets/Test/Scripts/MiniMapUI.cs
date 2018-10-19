using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class MiniMapUI : BaseUI
{
    public MiniMapUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
        _NaviData._IsCloseCoexistingUI = false;
    }
}
