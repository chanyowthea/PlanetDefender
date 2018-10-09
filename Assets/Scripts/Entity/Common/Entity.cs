using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public EFaction Faction{ protected set; get;}
    public virtual int HP { set; get; }
    public int MaxHP { protected set; get;}
    protected int _Defense = 2;
    protected float _MoveSpeed = 2;
    protected Vector3 _MoveDir;

    protected virtual void Update()
    {
        transform.position += _MoveDir * _MoveSpeed * GameManager.instance._Timer._TimeScale;
    }

    public virtual void Init()
    {

    }
}
