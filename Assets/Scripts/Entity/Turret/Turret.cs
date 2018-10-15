using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Army, IShot
{
    public int _Degree { get; protected set; }
    public Action<int> _onDie;
    [SerializeField] CustomImage _hpSprite;

    [SerializeField] Canvas _Canvas;
    public Canvas Canvas_
    {
        get
        {
            return _Canvas;
        }
    }

    public override int HP
    {
        set
        {
            //var c = _hpSprite.color;
            //c.a = value / (float)MaxHP;
            //_hpSprite.color = c;
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

    public int TurrectID { private set; get; }
    [SerializeField] Image _MaskImage;
    uint _DelayCallID;

    protected virtual void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.AttackFromPlanet, this, "Attack");
    }

    protected virtual void OnDestroy()
    {
        if (_DelayCallID != 0)
        {
            GameManager.instance.CancelCallEveryFrameInAPeriod(_DelayCallID);
            _DelayCallID = 0;
        }
        if (_MaskImage.material != null)
        {
            GameObject.Destroy(_MaskImage.material);
        }
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
            //var lastSize = _hpSprite.size;
            _hpSprite.sprite = GameObject.Instantiate(sprite);
            AdjustSize(_hpSprite.ImageSize);
            //_hpSprite.size = lastSize; 
        }
        _Attack = csv._Attack;
        _Defense = csv._Defense;
        TurrectID = turrectId;

        _MaskImage.material = null;
        _MaskImage.material = GameObject.Instantiate(GameAssets.instance._RatioRectMaterial);
    }

    protected void Attack()
    {
        Fire();
    }

    public virtual void Fire()
    {
        if (GameManager.instance._DelayCallUtil.GameTime - _LastFireTime <= _FireCoolDownTime)
        {
            return;
        }

        // mask effect
        if (_DelayCallID != 0)
        {
            return;
        }

        // Lightning
        if(TurrectID == 3)
            {
        Ray ray = new Ray(PlanetController.instance.transform.position, this.transform.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debugger.LogError("ray cast name=" + hit.collider.name);
        }
        }
        float maxTime = _FireCoolDownTime;
        _DelayCallID = GameManager.instance.CallEveryFrameInAPeriod(maxTime, (time) =>
        {
            _MaskImage.material.SetFloat("_Ratio", (maxTime - time) / maxTime);
        }, () => _DelayCallID = 0);
        _LastFireTime = GameManager.instance._DelayCallUtil.GameTime;
        var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
        bullet.SetData(FirePos, transform.up, _BulletMoveSpeed, _Attack, EFaction.Ours, TurrectID);
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

    public void OnClick()
    {
        Debugger.LogGreen("OnClick to fire! ");
        Fire();
    }

    [SerializeField] BoxCollider _BoxCollider;
    public void AdjustSize(Vector2 size)
    {
        _BoxCollider.size = size * 0.01f;
        (_Canvas.transform as RectTransform).sizeDelta = size;

        _MaskImage.rectTransform.sizeDelta = new Vector2(size.y, size.x);
    }
}
