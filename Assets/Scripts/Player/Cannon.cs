using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
    public float _fireSpeed = 1f;
    public float _fireRange = 10f;
    public float _fireCoolDownTime = 1f;
    public float _bulletMoveSpeed = 0.05f;
    public float _halfHeight = 0.5f;
    float _lastFireTime;
    public Vector3 firePos
    {
        get
        {
            // 炮塔半高,子弹半径
            return transform.position + transform.up * (_halfHeight + 0.15f / 2);
        }
    }

    [SerializeField] SpriteRenderer _hpSprite;
    int _hp;
    public int hp
    {
        set
        {
            var c = _hpSprite.color;
            c.a = value / (float)_maxHP;
            _hpSprite.color = c;
            _hp = value;
            if (hp <= 0)
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
            return _hp;
        }
    }
    public int _maxHP = 2;

    public int _attack = 1;
    public int _defense = 1;
    public int _degree;

    public Vector3 rotatePos
    {
        get
        {
            // 炮塔半高,子弹半径
            return transform.position - transform.up * _halfHeight;
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

    public Action<int> _onDie;

    void Attack()
    {
        Fire();
    }

    List<Rock> _rocksInFireRangle = new List<Rock>();
    Dictionary<Rock, Vector3> _rocksInMonitoring = new Dictionary<Rock, Vector3>();

    //protected virtual void Update()
    //{
    //    // 清空监视rock
    //    _rocksInMonitoring.Clear(); 
    //    // 还原攻击范围内的rock
    //    for (int i = 0, length = _rocksInFireRangle.Count; i < length; i++)
    //    {
    //        var r = _rocksInFireRangle[i];
    //        if (r == null)
    //        {
    //            continue; 
    //        }
    //        //r.mat = GameAssets.instance._rockMat_Default; 
    //    }
    //    _rocksInFireRangle.Clear();
    //    CheckFireRangeRocks(); 
    //    FireMonitoringRocks(); 
    //}

    void CheckFireRangeRocks()
    {
        // 检测所有攻击范围内的rock
        for (int i = 0, length = _detectAngle / 1; i < length; i++)
        {
            var v3 = V3RotateAround(rotatePos, -transform.forward, -_detectAngle / 2 + i * 1);
            RaycastHit hit;
            //Debug.DrawRay(firePos, v3 * _fireRange, Color.red);
            if (Physics.Raycast(firePos, v3, out hit, _fireRange))
            {
                var r = hit.collider.gameObject.GetComponent<Rock>();
                if (r != null)
                {
                    // 如果有rock进入,那么改变颜色
                    //r.mat = GameAssets.instance._rockMat;
                    if (!_rocksInFireRangle.Contains(r))
                    {
                        _rocksInFireRangle.Add(r);
                    }
                    // 获得rock的位移路线和开火路线的交点
                    var pos = PositionUtil.GetTargetPos(r.transform.position, r._moveDir, firePos, transform.up);
                    //Debug.LogFormat("code={0}, dis={1}, pos={2}", r.GetHashCode(), 
                    //    Vector3.Distance(pos, PlanetController.instance.transform.position), pos);
                    // 超出射击范围的不加入表中
                    if (Vector3.Distance(pos, PlanetController.instance.transform.position) > _fireRange + r._radius)
                    {
                        continue;
                    }
                    // 中心不在射击路径内的并且自身的collider也不在射击路径内的不加入表中
                    if (Vector3.Angle((pos - firePos), transform.up) > 90
                        && Vector3.Distance(pos, firePos) >= r._radius)
                    {
                        continue;
                    }

                    // 计算子弹从开火位置到交点位置的时间
                    float time = PositionUtil.GetTimeToTargetPos(firePos, pos, _bulletMoveSpeed);
                    // 计算rock到达交点前上述时间的位置
                    var rockPosWhileFire = PositionUtil.GetTargetPos(pos, -r._moveDir, r._moveSpeed, time);
                    // 将该rock加入监视列表,在后续如果rock到达指定位置,那么开火
                    if (_rocksInMonitoring.ContainsKey(r))
                    {
                        _rocksInMonitoring.Remove(r);
                    }
                    _rocksInMonitoring.Add(r, rockPosWhileFire);
                }
            }
        }
    }

    void FireMonitoringRocks()
    {
        // 查找监视的rock的位置,如果到达指定位置,那么开炮
        foreach (var item in _rocksInMonitoring)
        {
            if (Vector3.Distance(item.Key.transform.position, item.Value) < item.Key._radius / 1.41f) // item.Key._radius
            {
                if (Time.time - _lastFireTime <= _fireCoolDownTime)
                {
                    return;
                }

                Debug.DrawRay(firePos, item.Key.transform.position, Color.green, 5);
                Fire();
            }
        }
    }

    public virtual void Fire()
    {
        if (Time.time - _lastFireTime <= _fireCoolDownTime)
        {
            return;
        }
        _lastFireTime = Time.time;
        var bullet = GameObject.Instantiate(GameAssets.instance._bulletPrefab);
        bullet._moveDir = transform.up;
        bullet._startPos = firePos;
        bullet._moveSpeed = _bulletMoveSpeed;
        bullet._attack = _attack;
    }

    // 此处需要优化,可以预估陨石的移动位置并射击
    // 检测角度应该加大
    [SerializeField] int _detectAngle = 180;
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

    void OnTriggerEnter(Collider collider)
    {
		Debug.LogError("cannon target collider name=" + collider.name);
        var c = collider.gameObject.GetComponent<Rock>();
        if (c != null)
        {
			hp -= BattleUtil.CalcDamage(c._attack, _defense);
			Debug.LogError("cannon target damage=" + BattleUtil.CalcDamage(c._attack, _defense));
        }
    }
}
