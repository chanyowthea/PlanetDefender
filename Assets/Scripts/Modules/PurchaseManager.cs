using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class PurchaseManager : TSingleton<PurchaseManager>
{
    Dictionary<int, int> _MallStock = new Dictionary<int, int>();

    public override void Init()
    {
        base.Init();

        var csvs = ConfigDataManager.instance.GetDataList<OreCSV>();
        for (int i = 0, length = csvs.Count; i < length; i++)
        {
            var csv = csvs[i];
            int id = 0;
            int.TryParse(csv.GetPrimaryKey(), out id);
            if (id != 0)
            {
                _MallStock.Add(id, 99);
            }
        }
    }

    public override void Clear()
    {
        _MallStock.Clear(); 
        base.Clear();
    }

    public int GetStockCount(int id)
    {
        if (!_MallStock.ContainsKey(id))
        {
            return 0;
        }
        return _MallStock[id];
    }

    /// <returns>error code</returns>
    public int BuyItem(int id, int num)
    {
        var csv = ConfigDataManager.instance.GetData<OreCSV>(id.ToString());
        if (csv == null)
        {
            MessageManager.instance.ShowTips("Unknow error! ");
            Debugger.LogError("csv is empty! ");
            return 1;
        }
        if (ArchiveManager.instance.GetGoldCount() < csv._Price)
        {
            MessageManager.instance.ShowTips("gold is not enough! ");
            Debugger.LogError("gold is not enough! ");
            return 2;
        }
        EventDispatcher.instance.DispatchEvent(EventID.AddGold, -csv._Price);
        ArchiveManager.instance.ChangeMaterialsCount(id, num);
        return 0;
    }
}
