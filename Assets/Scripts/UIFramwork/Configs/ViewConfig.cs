using System;
using System.Collections;
using System.Collections.Generic;
using UIFramwork;
using UnityEngine;

public class ViewConfig : ScriptableObject
{
    public ELayer _layer;
    public BaseView _prefab;
    public ECloseType _closeType;
    public EOpenType _openType;
    public Type type
    {
        get
        {
            var v = _prefab;
            if (v == null)
            {
                return null; 
            }
            return v.GetType(); 
        }
    }
}
