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
                    _onDie(_degree);
                }
                Destroy(this.gameObject);
            }
        }
        get
        {
            return base.HP;
        }
    }

    protected virtual void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    void Attack()
    {
        Fire();
    }

    public void Fire()
    {
        if (Time.time - _lastFireTime <= _fireCoolDownTime)
        {
            return;
        }
        _lastFireTime = Time.time;
        var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
        bullet.SetData(firePos, transform.up, _bulletMoveSpeed, _attack, EFaction.Ours); 
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
