using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class TurretSelectItem : MonoBehaviour
{
    [SerializeField] CustomImage _Picture;
    [SerializeField] Text _NameText;
    int _Degree; 
    int _TurrectId; 

    public void SetData(string id, int degree)
    {
        if (string.IsNullOrEmpty(id))
        {
            return;
        }
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(id);
        if (csv == null)
        {
            return;
        }
        _TurrectId = int.Parse(id); 
        _NameText.text = csv._Name;
        _Picture.SetData(ResourcesManager.instance.GetSprite(csv._Picture)); 
        _Degree = degree; 
    }

    public void ClearData()
    {
        ResourcesManager.instance.UnloadAsset(_Picture.sprite);
        _Picture.sprite = null;
        _NameText.text = null;
        _Degree = 0; 
    }

    public void OnClickItem()
    {
        var ui = UIManager.Instance.Open<TurretDetailUI>();
        ui.SetData(_TurrectId, () =>
        {
            EventDispatcher.instance.DispatchEvent(EventID.CreateTurret, _Degree, _TurrectId);
        }); 
    }
}
