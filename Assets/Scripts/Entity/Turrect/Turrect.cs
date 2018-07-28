using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turrect : Army {

    public int _attack = 1;
    public int _degree;
    public float _fireSpeed = 1f;
    public float _fireRange = 10f;
    public float _fireCoolDownTime = 1f;
    public float _bulletMoveSpeed = 0.05f;
    protected float _lastFireTime;
    public float _halfHeight = 0.5f;
    public virtual Vector3 firePos
    {
        get
        {
            // 炮塔半高,子弹半径
            return transform.position + transform.up * (_halfHeight + 0.15f / 2);
        }
    }
}
