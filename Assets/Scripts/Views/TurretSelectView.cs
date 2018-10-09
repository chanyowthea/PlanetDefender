using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;

class TurretSelectView : BaseUI
{
    [SerializeField] RectTransform _ContentRtf;
    [SerializeField] TurretSelectItem _ItemPrefab;
    List<TurretSelectItem> _Items = new List<TurretSelectItem>();
    int _Degree;

    public TurretSelectView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public override void Open(NavigationData data)
    {
        base.Open(data);
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
    }

    internal override void Close()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.CreateTurretSuccess, this, "BuildSuccess");
        for (int i = 0, length = _Items.Count; i < length; i++)
        {
            var item = _Items[i];
            item.ClearData();
            GameObject.Destroy(item.gameObject);
        }
        _Items.Clear();
        base.Close();
    }

    void BuildSuccess(int degree, int turretId)
    {
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(turretId.ToString());
        if (csv == null)
        {
            Debugger.Log("cannot find csv data with id " + turretId);
            return;
        }
        var ui = UIManager.Instance.Open<MessageView>();
        ui.SetData(string.Format("Build {0} Success in Degree {1}! ", csv._Name, degree));
        UIManager.Instance.PopupLastFullScreenUI();
    }

    public void SetData(int degree)
    {
        _Degree = degree;
        _ItemPrefab.gameObject.SetActive(false);
        var list = ConfigDataManager.instance.GetDataList<TurretCSV>();
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
            item.SetData(info.GetPrimaryKey(), _Degree);
            _Items.Add(item);
        }
    }
}
