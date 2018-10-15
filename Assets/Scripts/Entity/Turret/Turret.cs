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

    public override float HP
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

    public int TurretID { private set; get; }
    [SerializeField] Image _MaskImage;
    uint _DelayCallID;
    UVChainLightning _Lightning;
    bool _LightningGunOpeningFire;

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
        TurretID = turrectId;
        _FireCoolDownTime = 1 / (float)csv._AttackSpeed; 

        _MaskImage.material = null;
        _MaskImage.material = GameObject.Instantiate(GameAssets.instance._RatioRectMaterial);

        if (TurretID == 3)
        {
            var line = this.gameObject.AddComponent<LineRenderer>();
            line.material = GameAssets.instance._LightningMaterial;
            _Lightning = this.gameObject.AddComponent<UVChainLightning>();
            _Lightning.startPos = this.transform.position;
            _Lightning.targetPos = Vector3.zero;
        }

        Debugger.LogRed("Distance=" + (Vector3.Distance(PlanetController.instance.transform.position, transform.position))); 
    }

    int GetMaxAngle(float distance)
    {
        return (int)Mathf.Lerp(60, 6, distance / (_FireRange - _HalfHeight));
    }

    uint _LightningFireDelayCall;
    protected override void Update()
    {
        //int angle = 60;
        //for (int i = 0; i < angle / 2; i++)
        //{
        //    Ray ray = new Ray(PlanetController.instance.transform.position, V3RotateAround(transform.up, -transform.forward, i));
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, _FireRange, LayerMask.GetMask("Enemy")))
        //    {
        //        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //        if (i * 2 <= GetMaxAngle(Vector3.Distance(hit.transform.position, transform.position)))
        //        {
        //            Debugger.LogError("caught a poor guy! " + hit.transform.name);
        //        }
        //    }
        //}
        //for (int i = 0; i < angle / 2; i++)
        //{
        //    Ray ray = new Ray(PlanetController.instance.transform.position, V3RotateAround(transform.up, -transform.forward, -i));
        //    RaycastHit hit;
        //    if (Physics.Raycast(ray, out hit, _FireRange, LayerMask.GetMask("Enemy")))
        //    {
        //        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
        //        if (i * 2 <= GetMaxAngle(Vector3.Distance(hit.transform.position, transform.position)))
        //        {
        //            Debugger.LogError("caught a poor guy! " + hit.transform.name);
        //        }
        //    }
        //}
        //return;


        if (_LightningGunOpeningFire)
        {
            _Lightning.startPos = this.transform.position;
            var target = GetLightningFireTarget();
            if (target != null)
            {
                var entity = target.GetComponent<Entity>();
                if (entity != null)
                {
                    var value = BattleUtil.CalcDamage(_Attack, entity._Defense);
                    entity.HP -= value * GameManager.instance._DelayCallUtil.Timer.DeltaTime;
                }
                _Lightning.targetPos = target.position;
                GameManager.instance.CancelDelayCall(_LightningFireDelayCall);
            }
            else
            {
                //Vector3 v = (this.transform.position - PlanetController.instance.transform.position) * _FireRange;
                //_Lightning.targetPos = v;
                //_LightningFireDelayCall = GameManager.instance.DelayCall(0.1f, () =>
                //{
                CancelLightningFire();
                //});
            }
        }
    }

    void CancelLightningFire()
    {
        _Lightning.targetPos = Vector3.zero;
        if (_LightningFireDelayCall != 0)
        {
            GameManager.instance.CancelDelayCall(_LightningFireDelayCall);
        }
        _LightningGunOpeningFire = false;
    }

    protected void Attack()
    {
        Fire();
    }

    // 此处需要优化,可以预估陨石的移动位置并射击
    // 检测角度应该加大
    //[SerializeField] int _detectAngle = 180;
    //public virtual bool ExistEnemies()
    //{
    //    RaycastHit hit;
    //    Debug.DrawRay(transform.position, transform.up, Color.red);

    //    for (int i = 0, length = _detectAngle / 1; i < length; i++)
    //    {
    //        var v3 = V3RotateAround(transform.up, -transform.forward, -_detectAngle / 2 + i * 1);
    //        if (Physics.Raycast(transform.position, transform.up, out hit, _fireRange))
    //        {
    //            //Debug.Log("collider name=" + hit.collider.gameObject.name);
    //            return hit.collider.gameObject.tag == "Goods";
    //        }
    //    }
    //    return false; 
    //}

    /// <summary>
    /// 计算一个Vector3绕旋转中心旋转指定角度后所得到的向量。
    /// </summary>
    /// <param name="source">旋转前的源Vector3</param>
    /// <param name="axis">旋转轴</param>
    /// <param name="angle">旋转角度</param>
    /// <returns>旋转后得到的新Vector3</returns>
    public Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
        return q * source;// 返回目标点
    }

    Transform GetLightningFireTarget(int angle = 60)
    {
        // half angle degree 
        for (int i = 0; i < angle / 2; i++)
        {
            Ray ray = new Ray(PlanetController.instance.transform.position, V3RotateAround(transform.up, -transform.forward, i));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _FireRange, LayerMask.GetMask("Enemy")))
            {
                if (i * 2 <= GetMaxAngle(Vector3.Distance(hit.transform.position, transform.position)))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                    return hit.transform;
                }
            }
        }
        for (int i = 0; i < angle / 2; i++)
        {
            Ray ray = new Ray(PlanetController.instance.transform.position, V3RotateAround(transform.up, -transform.forward, -i));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, _FireRange, LayerMask.GetMask("Enemy")))
            {
                if (i * 2 <= GetMaxAngle(Vector3.Distance(hit.transform.position, transform.position)))
                {
                    //Debugger.LogError("caught a poor guy! " + hit.transform.name);
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                    return hit.transform;
                }
            }
        }
        return null;
    }

    public virtual void Fire()
    {
        if (GameManager.instance._DelayCallUtil.GameTime - _LastFireTime <= _FireCoolDownTime)
        {
            return;
        }

        // Lightning
        if (TurretID == 3)
        {
            bool inFireRange = GetLightningFireTarget() != null;
            if (!inFireRange)
            {
                MessageView v = UIFramework.UIManager.Instance.Open<MessageView>();
                v.SetData("No enemy in fire range! ");
                return;
            }
            CancelLightningFire();
            _Lightning.startPos = this.transform.position;
            _LightningGunOpeningFire = true;
        }
        else
        {
            var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
            bullet.SetData(FirePos, transform.up, _BulletMoveSpeed, _Attack, EFaction.Ours, TurretID);
        }

        // mask effect
        if (_DelayCallID != 0)
        {
            GameManager.instance.CancelCallEveryFrameInAPeriod(_DelayCallID);
        }
        float maxTime = _FireCoolDownTime;
        _DelayCallID = GameManager.instance.CallEveryFrameInAPeriod(maxTime, (time) =>
        {
            _MaskImage.material.SetFloat("_Ratio", (maxTime - time) / maxTime);
        }, () => _DelayCallID = 0);
        _LastFireTime = GameManager.instance._DelayCallUtil.GameTime;
    }

    // attack by other 
    protected virtual void OnTriggerEnter(Collider collider)
    {
        var c = collider.gameObject.GetComponent<Enemy>();
        if (c != null && !GameConfig.instance._TurretImmuneDamage)
        {
            HP -= BattleUtil.CalcDamage(c.Attack, _Defense);
        }
    }

    public void OnClick()
    {
        Fire();
    }

    [SerializeField] BoxCollider _BoxCollider;
    public void AdjustSize(Vector2 size)
    {
        _BoxCollider.size = size * 0.01f;
        (_Canvas.transform as RectTransform).sizeDelta = size;

        _MaskImage.rectTransform.sizeDelta = new Vector2(size.y, size.x);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(PlanetController.instance.transform.position, 
            _FireRange + Vector3.Distance(PlanetController.instance.transform.position, transform.position));
    }
#endif
}
