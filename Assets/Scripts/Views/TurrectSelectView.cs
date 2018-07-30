using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;
using UnityEngine.UI;

public class TurrectSelectView : BaseView
{
    [SerializeField] RectTransform _ContentRtf;
    [SerializeField] TurrectSelectItem _ItemPrefab;
    List<TurrectSelectItem> _Items = new List<TurrectSelectItem>();

    public override void Open()
    {
        base.Open();
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

    public override void Close()
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

    // 有没有办法直接用内部的Close就直接能实现功能，不用下面这行代码
    // UI框架需要重构
    public void OnClickBack()
    {
        ViewManager.instance.Close(GetHashCode());
    }
}
