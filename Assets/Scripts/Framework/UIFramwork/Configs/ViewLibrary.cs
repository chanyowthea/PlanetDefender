using System;
using System.Collections;
using System.Collections.Generic;
using UIFramwork;
using UnityEngine;

public class ViewLibrary : ScriptableObject
{
    public ViewConfig[] _configs;

    public ViewConfig GetConfig(Type type)
    {
        if (type == null)
        {
            Debug.LogError("type is error! ");
            return null; 
        }

        for (int i = 0, length = _configs.Length; i < length; i++)
        {
            var c = _configs[i];
            if (c == null)
            {
                Debug.LogError("config is empty! i=" + i); 
                continue; 
            }
            if (type == c.type)
            {
                return c; 
            }
        }
        return null; 
    }
}
