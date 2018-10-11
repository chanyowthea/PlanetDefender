using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class MallItem : MonoBehaviour, ILoopScrollRectItem<int>
{
    [SerializeField] Text _NameText;
    [SerializeField] CustomImage _CustomImage;
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
        var ui = UIManager.Instance.Open<PromptView>();
        ui.SetData("Confirm to buy this one? ", () =>
        {
            ArchiveManager.instance.ChangeMaterialsCount(_ID, 1);
            Debugger.Log("buy item with id " + _ID + " successful! "); 
        });
    }
}
