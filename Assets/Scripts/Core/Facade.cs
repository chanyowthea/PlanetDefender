using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class Facade : TSingleton<Facade>
{
    public void ChangeScene(string sceneName)
    {
        UIManager.Instance.ChangeScene();
        var load = UIManager.Instance.Open<LoadingView>();
        load.SetData(sceneName);
    }
}

