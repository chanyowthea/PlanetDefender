using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LocComponent : MonoBehaviour
{
    public static List<LocComponent> _Locs = new List<LocComponent>();
    private Text _Text;

    public string StringID;
    void Start()
    {
        _Text = gameObject.GetComponent<Text>();
        Process();
        _Locs.Add(this);
    }

    public void Process()
    {
        if (!string.IsNullOrEmpty(StringID))
        {
            _Text.text = LocManager.instance.DoLoc(StringID);
        }
    }
}
