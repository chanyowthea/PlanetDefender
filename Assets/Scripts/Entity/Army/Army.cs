using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army : Entity
{
    protected int _Attack = 1;
    protected float _FireSpeed = 1f;
    protected float _FireRange = 10f;
    protected float _FireCoolDownTime = 1f;
    protected float _BulletMoveSpeed = 0.05f;
    protected float _LastFireTime;
    protected float _HalfHeight = 0.5f;
    public virtual Vector3 FirePos
    {
        get
        {
            // 炮塔半高,子弹半径
            return transform.position + transform.up * (_HalfHeight + 0.15f / 2);
        }
    }
    public virtual float VisualField { get; protected set;}

    public override void Init()
    {
        base.Init();
    }
}
