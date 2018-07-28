using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BaseMove
{
    //public IShot _parent;
    public EFaction _Faction;
    public Vector3 _startPos;
    public float _radius = 0.075f;
    public int _attack = 1;
    [SerializeField] GameObject _bulletTf;

    void Awake()
    {
        transform.SetParent(GameAssets.bulletParent.transform);
    }

    private void Start()
    {
        transform.position = _startPos;

        var angle = Vector3.Angle(transform.up, _moveDir);
        if (_moveDir.x > 0)
        {
            angle = 360 - angle;
        }
        Debug.Log("moveDir=" + _moveDir + ", angle=" + angle);
        var v3 = V3RotateAround(transform.up, -transform.forward, angle);
        _bulletTf.transform.localEulerAngles = new Vector3(0, 0, angle);
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
        if (entity != null && entity.Faction != _Faction)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
