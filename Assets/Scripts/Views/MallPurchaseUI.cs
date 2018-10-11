using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class MallPurchaseUI : BaseUI
{
    int _Number = 1;
    int Number
    {
        get
        {
            int.TryParse(_NumberInput.text, out _Number);
            return _Number;
        }
        set
        {
            if (value > _MaxNumber)
            {
                value = _MaxNumber;
            }
            if (value < 1)
            {
                value = 1;
            }

            _Number = value;
            _NumberInput.text = value.ToString();
            if (_CSVData != null)
            {
                _TotalCostText.text = (_Number * _CSVData._Price).ToString();
            }
        }
    }

    [SerializeField] Text _MessageText;
    [SerializeField] InputField _NumberInput;
    [SerializeField] Text _StockText;
    [SerializeField] Text _TotalCostText;
    Action _OnOK;
    int _ID;
    int _MaxNumber = 1;
    OreCSV _CSVData;

    public MallPurchaseUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
    }

    public void OnClickClose()
    {
        _NumberInput.onEndEdit.RemoveListener(OnEndEdit);
        UIManager.Instance.Close(this);
    }

    public void OnClickOK()
    {
        var code = PurchaseManager.instance.BuyItem(_ID, Number);
        if (code == 0)
        {
            if (_OnOK != null)
            {
                _OnOK();
            }
            UIManager.Instance.Close(this);
        }
    }

    public void SetData(string msg, Action onOK, int id, int maxNumber)
    {
        _MessageText.text = msg;
        _OnOK = onOK;
        _MaxNumber = maxNumber;
        _StockText.text = maxNumber.ToString();
        _ID = id;
        _CSVData = ConfigDataManager.instance.GetData<OreCSV>(_ID.ToString());
        _NumberInput.onEndEdit.AddListener(OnEndEdit);
        Number = 1;
    }

    void OnEndEdit(string s)
    {
        Number = Number;
    }
}
