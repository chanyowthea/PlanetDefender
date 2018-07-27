using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    public float _moveSpeed = 1;
    public Vector3 _moveDir;

    protected virtual void Update()
    {
        transform.position += _moveDir * _moveSpeed * Time.timeScale;
    }
}
