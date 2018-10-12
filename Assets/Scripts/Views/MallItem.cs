using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class MallItem : MonoBehaviour, ILoopScrollRectItem<int>
{
    [SerializeField] Text _NameText;
    [SerializeField] CustomImage _CustomImage;
    [SerializeField] Text _StockText;
    [SerializeField] Text _PriceText;
    int _ID;

    public void SetData(int id)
    {
        _ID = id;
        var csv = ConfigDataManager.instance.GetData<OreCSV>(_ID.ToString());
        if (csv == null)
        {
            return;
        }
        _NameText.text = csv._Name;
        _PriceText.text = csv._Price.ToString();
        _StockText.text = PurchaseManager.instance.GetStockCount(id).ToString(); 
        var s = ResourcesManager.instance.GetSprite(csv._Picture);
        _CustomImage.SetData(s);
    }

    public void ClearData()
    {
        ResourcesManager.instance.UnloadAsset(_CustomImage.sprite);
        _CustomImage.sprite = null;
    }

    public void OnClickItem()
    {
        Debugger.Log("OnClickItem id=" + _ID);
        var ui = UIManager.Instance.Open<MallPurchaseUI>();
        ui.SetData("Confirm to buy this one? ", () =>
        {
            Debugger.Log("buy item with id " + _ID + " successful! "); 
        }, _ID, PurchaseManager.instance.GetStockCount(_ID));
    }
}
