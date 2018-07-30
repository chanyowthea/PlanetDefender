using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurrectSelectItem : MonoBehaviour
{
    [SerializeField] Image _Picture;
    [SerializeField] Text _NameText;

    public void SetData(string id)
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
        _NameText.text = csv._Name;
        _Picture.sprite = ResourcesManager.instance.GetSprite(csv._Picture);
    }

    public void ClearData()
    {
        _NameText.text = null;
        ResourcesManager.instance.UnloadAsset(_Picture.sprite);
        _Picture.sprite = null;
    }

    public void OnClickItem()
    {

    }
}
