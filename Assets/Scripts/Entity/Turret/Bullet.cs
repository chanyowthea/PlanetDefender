using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    public float _radius = 0.075f;
    public int Attack { protected set; get; }
    [SerializeField] GameObject _bulletTf;
    [SerializeField] SpriteRenderer _SpriteRenderer;

    void Awake()
    {
        transform.SetParent(GameAssets.bulletParent.transform);
    }

    public override void Init()
    {
        base.Init();

        var angle = Vector3.Angle(transform.up, _MoveDir);
        if (_MoveDir.x > 0)
        {
            angle = 360 - angle;
        }
        var v3 = V3RotateAround(transform.up, -transform.forward, angle);
        _bulletTf.transform.localEulerAngles = new Vector3(0, 0, angle);
    }

    private void OnDestroy()
    {
        if (_SpriteRenderer.sprite != null)
        {
            GameObject.Destroy(_SpriteRenderer.sprite);
        }
    }

    public void SetData(Vector3 pos, Vector3 moveDir, float moveSpeed, int attack, EFaction faction, int turretId)
    {
        transform.position = pos;
        _MoveDir = moveDir;
        _MoveSpeed = moveSpeed;
        Attack = attack;
        Faction = faction;

        _SpriteRenderer.sprite = null;
        TurretCSV csv = ConfigDataManager.instance.GetData<TurretCSV>(turretId.ToString());
        if (csv != null)
        {
            var sprite = ResourcesManager.instance.GetSprite(csv._BulletPicture);
            if (sprite != null)
            {
                var lastSize = _SpriteRenderer.size;
                _SpriteRenderer.sprite = GameObject.Instantiate(sprite);
                _SpriteRenderer.size = lastSize;
            }
        }
        Init();
    }

    public Vector3 V3RotateAround(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
        return q * source;// 返回目标点
    }

    protected override void Update()
    {
        base.Update();

        if (PlanetController.instance == null)
        {
            return;
        }

        if (!PlanetController.instance.IsInVisualField(transform.position))
        {
            Destroy(this.gameObject);
        }
    }

    // attack other
    void OnTriggerEnter(Collider collider)
    {
        var entity = collider.GetComponent<Entity>();
        if (entity != null && entity.Faction != Faction)
        {
            Debug.LogFormat("entity.name={0}, faction={1}", entity.name, entity.Faction);
            Debug.LogFormat("bullet.name={0}, faction={1}", this.gameObject.name, this.Faction);
            GameObject.Destroy(this.gameObject);
        }
    }
}
