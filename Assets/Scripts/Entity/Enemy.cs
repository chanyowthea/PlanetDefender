using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    public float _radius = 1.41f / 2;

    [SerializeField] Text _hpText;
    public override int HP
    {
        set
        {
            _hpText.text = value.ToString();
            //Debug.Log("Rock set hp=" + value);
            if (base.HP > 0 && value <= 0)
            {
                EventDispatcher.instance.DispatchEvent(EventID.AddScore, MaxHP);
                if (_EnemyType == EEnemyType.Bomb)
                {
                    Debugger.Log("play bomb effect! ", LogColor.Red);
                    var go = Instantiate(GameAssets.instance._ExplosionEffect);
                    go.transform.position = this.transform.position;
                    CoroutineUtil.instance.Wait(1, () =>
                    {
                        GameObject.Destroy(go);
                    });
                }
                Destroy(this.gameObject);
            }
            base.HP = value;
        }
        get
        {
            return base.HP;
        }
    }

    [SerializeField] SpriteRenderer _Sprite;
    public int Attack { private set; get; }
    EEnemyType _EnemyType;

    void Awake()
    {
        transform.SetParent(GameAssets.rockParent.transform); 
        gameObject.name = GetType() + "_" + GetHashCode();
    }

    public void SetData(Vector3 pos, float moveSpeed, Vector3 moveDir, EFaction faction, int enemyId)
    {
        _Sprite.sprite = null;
        transform.position = pos;
        _MoveSpeed = moveSpeed;
        _MoveDir = moveDir;
        Faction = faction;
        var csv = ConfigDataManager.instance.GetData<EnemyCSV>(enemyId.ToString());
        if (csv != null)
        {
            MaxHP = csv._MaxHP;
            Attack = csv._Attack;
            _Defense = csv._Defense;
            var sprite = ResourcesManager.instance.GetSprite(csv._Picture);
            if (sprite != null)
            {
                var lastSize = _Sprite.size;
                _Sprite.sprite = GameObject.Instantiate(sprite);
                _Sprite.size = lastSize;
            }
            _EnemyType = csv._Type;
        }
        HP = MaxHP;
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        base.Update();
        // 两个点中到星球的距离更近的点
        Vector3 neearestPos = Vector3.Min(transform.position - _MoveDir * _radius - PlanetController.instance.transform.position,
            transform.position - PlanetController.instance.transform.position);
        if (!PlanetController.instance.IsInVisualField(neearestPos))
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // 撞到星球就消失
        if (collider.gameObject.GetComponent<PlanetController>() != null)
        {
            HP = 0;
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
        if (_Sprite.sprite != null)
        {
            GameObject.Destroy(_Sprite.sprite);
        }
    }
}
