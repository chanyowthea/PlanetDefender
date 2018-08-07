using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class TurrectSelectView : BaseUI
{
    [SerializeField] RectTransform _ContentRtf;
    [SerializeField] TurrectSelectItem _ItemPrefab;
    List<TurrectSelectItem> _Items = new List<TurrectSelectItem>();
    
    public TurrectSelectView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        _ItemPrefab.gameObject.SetActive(false); 
        var list = ConfigDataManager.instance.GetDataList<TurrectCSV>();
        for (int i = 0, length = list.Count; i < length; i++)
        {
            var info = list[i];
            if (info == null)
            {
                continue;
            }
            var item = GameObject.Instantiate(_ItemPrefab, _ContentRtf);
            item.gameObject.SetActive(true); 
            item.transform.localScale = Vector3.one;
            item.SetData(info.GetPrimaryKey());
            _Items.Add(item);
        }
    }

    internal override void Close()
    {
        for (int i = 0, length = _Items.Count; i < length; i++)
        {
            var item = _Items[i];
            item.ClearData(); 
            GameObject.Destroy(item.gameObject); 
        }
        _Items.Clear(); 
        base.Close();
    }

    public void OnClickBack()
    {
        UIManager.Instance.Close(this);
    }
}
