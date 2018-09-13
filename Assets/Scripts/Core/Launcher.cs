using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

public class Launcher : MonoBehaviour
{
    void Start()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        SingletonManager.Init();
        ArchiveManager.instance.Init(); 
        ConfigDataManager.instance.LoadCSV<UICSV>("UI");
        UIManager.Instance.Open<StartView>();
    }
}
