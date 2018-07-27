using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : BaseMove
{
    public float _radius = 1.41f / 2;

    int _hp; 
    public int hp 
    {
        set
        {
            _hpText.text = value.ToString();
            _hp = value;
            if (hp <= 0)
            {
                EventDispatcher.instance.DispatchEvent(EventID.AddScore, _maxHP);
                Destroy(this.gameObject);
            }
        }
        get
        {
            return _hp; 
        }
    }
    public int _maxHP = 2;

    [SerializeField] Text _hpText;
    public int _attack = 3;
    public int _defense = 1;

    [SerializeField] SpriteRenderer _sprite;
    void Awake()
    {
        transform.SetParent(GameAssets.rockParent.transform);
        hp = _maxHP;
        gameObject.name = "" + GetHashCode(); 
    }

    public Material mat
    {
        set
        {
            _sprite.material = value; 
        }
    }

    protected override void Update()
    {
        base.Update();
        // 两个点中到星球的距离更近的点
        Vector3 neearestPos = Vector3.Min(transform.position - _moveDir * _radius - PlanetController.instance.transform.position, 
            transform.position - PlanetController.instance.transform.position); 
        if (!PlanetController.instance.IsInVisualField(neearestPos))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("target collider name=" + collider.name);
        // 撞到星球就消失
        if (collider.gameObject.GetComponent<PlanetController>() != null)
        {
            EventDispatcher.instance.DispatchEvent(EventID.AddScore, _maxHP);
            Destroy(this.gameObject);
        }
        // 撞到子弹会扣血
        else
        {
            ExecuteAttack(collider.gameObject.GetComponent<Bullet>());
        }
    }

    void ExecuteAttack(Bullet bullet)
    {
        if (bullet == null)
        {
            return; 
        }

        hp -= BattleUtil.CalcDamage(bullet._attack, _defense);
    }
}
