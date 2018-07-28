using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRotate : MonoBehaviour
{
    public float _rotateSpeedDegreePerFrame = 1; 

    private void Update()
    {
        transform.localEulerAngles += new Vector3(0, 0, _rotateSpeedDegreePerFrame * Time.timeScale);
    }
}
