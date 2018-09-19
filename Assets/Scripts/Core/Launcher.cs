using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        Facade.instance.Init(); 
        UIManager.Instance.Open<StartView>();
    }
}
