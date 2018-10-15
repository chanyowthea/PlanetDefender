using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EFaction Faction { protected set; get; }
    public virtual float HP { set; get; }
    public int MaxHP { protected set; get; }
    public int _Defense { protected set; get; }
    protected float _MoveSpeed = 2;
    protected Vector3 _MoveDir;

    protected virtual void Update()
    {
        transform.position += _MoveDir * _MoveSpeed * GameManager.instance.TimeScale;
    }

    public virtual void Init()
    {

    }
}
