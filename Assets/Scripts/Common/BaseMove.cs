using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMove : MonoBehaviour
{
    public float _MoveSpeed = 1;
    protected Vector3 _MoveDir;

    protected virtual void Update()
    {
        transform.position += _MoveDir * _MoveSpeed * GameManager.instance.TimeScale;
    }
}
