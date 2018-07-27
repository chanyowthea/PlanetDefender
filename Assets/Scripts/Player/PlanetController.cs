using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UIFramwork;

public class PlanetController : MonoSingleton<PlanetController>
{
    [SerializeField] Planet _planet;
    bool _isRotate;

    private void Start()
    {
        EventDispatcher.instance.RegisterEvent(EventID.RotatePlanet, this, "Rotate");
        EventDispatcher.instance.RegisterEvent(EventID.CreateTurret, this, "CreateTurret");
        EventDispatcher.instance.RegisterEvent(EventID.AddHealth, this, "AddHealth");
    }

    private void OnDestroy()
    {
        EventDispatcher.instance.UnRegisterEvent(EventID.AddHealth, this, "AddHealth");
        EventDispatcher.instance.UnRegisterEvent(EventID.RotatePlanet, this, "Rotate");
        EventDispatcher.instance.UnRegisterEvent(EventID.CreateTurret, this, "CreateTurret");
    }

    public void _Reset()
    {
        _planet.Init();
    }

    void Rotate(bool value)
    {
        _planet.rotate.enabled = value;
    }

    void CreateTurret(int degree)
    {
        _planet.CreateCannon(degree);
    }

    void AddHealth(int value)
    {
        if (_planet.hp + value < _planet._maxHP)
        {
            _planet.hp += value;
        }
        else
        {
            _planet.hp = _planet._maxHP; 
        }
    }

    void Update()
    {
        if (_planet == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _planet.CreatCannonQuick();
        }
    }

    public bool IsInVisualField(Vector3 pos)
    {
        if (_planet == null)
        {
            return false;
        }
        return _planet.IsInVisualField(pos);
    }

    void OnTriggerEnter(Collider collider)
    {
        var gold = collider.gameObject.GetComponent<Gold>();
        if (gold != null)
        {
            EventDispatcher.instance.DispatchEvent(EventID.AddGold, gold.hp);
        }
    }

    public Cannon GetCannonByDegree(int degree)
    {
        if (_planet.cannonPivotDict.ContainsKey(degree))
        {
            var c = _planet.cannonPivotDict[degree];
            if (c == null)
            {
                return null;
            }
            var comp = c.GetComponentInChildren<Cannon>();
            return comp;
        }
        return null;
    }

    public Cannon[] GetAllCannons()
    {
        List<Cannon> cs = new List<Cannon>();
        foreach (var item in _planet.cannonPivotDict)
        {
            if (item.Value == null)
            {
                continue;
            }
            var comp = item.Value.GetComponentInChildren<Cannon>();
            if (comp == null)
            {
                continue;
            }
            cs.Add(comp);
        }
        return cs.ToArray();
    }

    public int GetHP()
    {
        if (_planet == null)
        {
            return 0;
        }
        return _planet.hp;
    }

    public bool IsHpLessThanMax()
    {
        if (_planet == null)
        {
            return false;
        }
        return _planet.hp < _planet._maxHP;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _planet.visualField);
    }
#endif
}
