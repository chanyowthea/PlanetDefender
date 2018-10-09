using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class MallUI : BaseUI
{
    [SerializeField] LoopVerticalScrollRect _Rect; 

    public MallUI()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true; 
    }

    public override void Open(NavigationData data = null)
    {
        base.Open(data);
        var list = ConfigDataManager.instance.GetDataList<OreCSV>(); 
        List<int> ids = new List<int>();
        int id = 0;
        for (int i = 0, length = list.Count; i < length; i++)
        {
            var csv = list[i] as OreCSV;
            if (int.TryParse(csv.GetPrimaryKey(), out id) && csv._InMall)
            {
                ids.Add(id);
            }
        }
        _Rect.SetData(ids.ToArray()); 
        _Rect.RefillCells(); 
    }

    internal override void Close()
    {
        var ui = UIManager.Instance.GetCurrentResidentUI<TopResidentUI>();
        if (ui != null)
        {
            ui.UpdateView(true);
        }
        _Rect.ClearCells(); 
        base.Close();
    }
}
