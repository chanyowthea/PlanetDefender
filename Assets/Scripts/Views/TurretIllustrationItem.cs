using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class TurretIllustrationItem : MonoBehaviour, ILoopScrollRectItem<int>
{
    [SerializeField] Text _NameText;
    [SerializeField] CustomImage _CustomImage;
    int _ID;
    
    public void SetData(int id)
    {
        _ID = id;
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(_ID.ToString());
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
    }
}
