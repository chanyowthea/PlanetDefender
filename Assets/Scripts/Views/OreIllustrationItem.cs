using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class OreIllustrationItem : MonoBehaviour, ILoopScrollRectItem<int>
{
    [SerializeField] Text _NameText;
    [SerializeField] CustomImage _CustomImage;
    int _OreId;
    
    public void SetData(int oreId)
    {
        _OreId = oreId;
        var csv = ConfigDataManager.instance.GetData<OreCSV>(_OreId.ToString());
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

    }

    public void OnClickItem()
    {
        DebugFramework.Debugger.Log("OnClickItem id=" + _OreId); 
    }
}
