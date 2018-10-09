using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Army, IShot
{
    public int _Degree { get; protected set; }
    public Action<int> _onDie;
    [SerializeField] SpriteRenderer _hpSprite;

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

    public int TurrectID{ private set; get; }

    protected virtual void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    protected virtual void OnDestroy()
    {
        if (_hpSprite.sprite != null)
        {
            GameObject.Destroy(_hpSprite.sprite);
        }
        EventDispatcher.instance.UnRegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    public virtual void SetData(int degree, EFaction faction, int turrectId)
    {
        _hpSprite.sprite = null; 
        _Degree = degree; 
        Faction = faction;

        var csv = ConfigDataManager.instance.GetData<TurretCSV>(turrectId.ToString());
        if (csv == null)
        {
            Debug.LogError("CreateCannon csv is empty! ");
            return;
        }
        var sprite = ResourcesManager.instance.GetSprite(csv._Picture);
        if (sprite != null)
        {
            var lastSize = _hpSprite.size;
            _hpSprite.sprite = GameObject.Instantiate(sprite);
            _hpSprite.size = lastSize; 
        }
        _Attack = csv._Attack; 
        _Defense = csv._Defense; 
        TurrectID = turrectId; 
    }

    protected void Attack()
    {
        Fire();
    }

    public virtual void Fire()
    {
        if (GameManager.instance._Timer.GameTime - _LastFireTime <= _FireCoolDownTime)
        {
            return;
        }
        _LastFireTime = GameManager.instance._Timer.GameTime;
        var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
        bullet.SetData(FirePos, transform.up, _BulletMoveSpeed, _Attack, EFaction.Ours);
    }

    // attack by other 
    protected virtual void OnTriggerEnter(Collider collider)
    {
        var c = collider.gameObject.GetComponent<Enemy>();
        if (c != null)
        {
            HP -= BattleUtil.CalcDamage(c.Attack, _Defense);
        }
    }
}
