using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : Turrect, IShot
{
    [SerializeField] SpriteRenderer _hpSprite;
    List<Rock> _rocksInFireRangle = new List<Rock>();
    public Action<int> _onDie;
    Dictionary<Rock, Vector3> _rocksInMonitoring = new Dictionary<Rock, Vector3>();

    public override int HP
    {
        set
        {
            var c = _hpSprite.color;
            c.a = value / (float)MaxHP;
            _hpSprite.color = c;
            base.HP = value;
            if (HP <= 0)
            {
                if (_onDie != null)
                {
                    _onDie(_Degree);
                }
                Destroy(this.gameObject);
            }
        }
        get
        {
            return base.HP;
        }
    }

    void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    void OnDestroy()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    public void SetData(int degree, EFaction faction)
    {
        _Degree = degree; 
        Faction = faction;
    }

    void Attack()
    {
        Fire();
    }

    public void Fire()
    {
        if (Time.time - _LastFireTime <= _FireCoolDownTime)
        {
            return;
        }
        _LastFireTime = Time.time;
        var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
        bullet.SetData(FirePos, transform.up, _BulletMoveSpeed, _Attack, EFaction.Ours);
    }

    // attack by other 
    void OnTriggerEnter(Collider collider)
    {
        var c = collider.gameObject.GetComponent<Rock>();
        if (c != null)
        {
            HP -= BattleUtil.CalcDamage(c.attack, _Defense);
        }
    }
}
