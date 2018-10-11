using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class InventoryUI : BaseUI
{
    [SerializeField] LoopVerticalScrollRect _Rect; 

    public InventoryUI()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true; 
    }

    public override void Open(NavigationData data = null)
    {
        base.Open(data);
        var dict = ArchiveManager.instance.AccountInfo._Materials.GetDictionary();
        var list = new List<int>(dict.Keys); 
        _Rect.SetData(list.ToArray()); 
        _Rect.RefillCells();

        var ui = UIManager.Instance.GetCurrentResidentUI<TopResidentUI>();
        if (ui != null)
        {
            ui.UpdateView(true);
        }
    }

    internal override void Close()
    {
        _Rect.ClearCells(); 
        base.Close();
    }
}
