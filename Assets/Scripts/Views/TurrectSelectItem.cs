using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class TurrectSelectItem : MonoBehaviour
{
    [SerializeField] Image _Picture;
    [SerializeField] Text _NameText;
    int _Degree; 
    int _TurrectId; 

    public void SetData(string id, int degree)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }
        var csv = ConfigDataManager.instance.GetData<TurrectCSV>(id);
        if (csv == null)
        {
            return;
        }
        _TurrectId = int.Parse(id); 
        _NameText.text = csv._Name;
        _Picture.sprite = ResourcesManager.instance.GetSprite(csv._Picture); 
        _Degree = degree; 
    }

    public void ClearData()
    {
        _NameText.text = null;
        ResourcesManager.instance.UnloadAsset(_Picture.sprite);
        _Picture.sprite = null;
        _Degree = 0; 
    }

    public void OnClickItem()
    {
        EventDispatcher.instance.DispatchEvent(EventID.CreateTurret, _Degree, _TurrectId);
        UIManager.Instance.Close<TurrectSelectView>(); 
    }
}
