using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Entity
{
    public float _radius = 0.075f;
    public int Attack { protected set; get; }
    [SerializeField] GameObject _bulletTf;

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

    public void SetData(Vector3 pos, Vector3 moveDir, float moveSpeed, int attack, EFaction faction)
    {
        transform.position = pos;
        _MoveDir = moveDir;
        _MoveSpeed = moveSpeed;
        Attack = attack;
        Faction = faction;
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

    private void OnDestroy()
    {
        Debug.LogError("OnDestroy Bullet");
    }
}
