using UnityEngine;
using System.Collections;
// 将脚本挂载到摄像机上  
public class CameraController : MonoBehaviour
{
    public Camera _MainCamera{ private set; get;}
    public static CameraController Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
        _MainCamera = GetComponent<Camera>(); 
    }
}