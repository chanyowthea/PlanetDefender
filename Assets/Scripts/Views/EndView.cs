using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramwork;
using UnityEngine.UI;

public class EndView : BaseView
{
    [SerializeField] Text _resultText;
    public void OnClickEnd()
    {
        ViewManager.instance.Open<StartView>(true);
    }

    public void SetData(string result)
    {
        _resultText.text = result;
    }
}
