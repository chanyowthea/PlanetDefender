using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : Entity
{
    public float _radius = 1.41f / 2;

    [SerializeField] Text _hpText;
    public override int HP
    {
        set
        {
            _hpText.text = value.ToString();
            base.HP = value;
            if (HP <= 0)
            {
                EventDispatcher.instance.DispatchEvent(EventID.AddScore, MaxHP);
                Destroy(this.gameObject);
            }
        }
        get
        {
            return base.HP;
        }
    }

    [SerializeField] SpriteRenderer _sprite;
    public int attack { private set; get;}

    void Awake()
    {
        transform.SetParent(GameAssets.rockParent.transform);
        HP = MaxHP;
        gameObject.name = "" + GetHashCode();
    }
    
    public void SetData(Vector3 pos, float moveSpeed, Vector3 moveDir, EFaction faction)
    {
        transform.position = pos;
        _MoveSpeed = moveSpeed;
        _MoveDir = moveDir;
        Faction = faction;
        Init(); 
    }

    protected override void Update()
    {
        base.Update();
        // 两个点中到星球的距离更近的点
        Vector3 neearestPos = Vector3.Min(transform.position - _MoveDir * _radius - PlanetController.instance.transform.position,
            transform.position - PlanetController.instance.transform.position);
        if (!PlanetController.instance.IsInVisualField(neearestPos))
        {
            Debug.Log("Update=");
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("target collider name=" + collider.name);
        // 撞到星球就消失
        if (collider.gameObject.GetComponent<PlanetController>() != null)
        {
            Debug.LogError("Rock OnTriggerEnter");
            EventDispatcher.instance.DispatchEvent(EventID.AddScore, MaxHP);
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

        HP -= BattleUtil.CalcDamage(bullet.Attack, _Defense);
    }

    private void OnDestroy()
    {
        //Debug.LogError("Rock OnDestroy");
    }
}
