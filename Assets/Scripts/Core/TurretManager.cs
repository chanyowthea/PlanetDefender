using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class TurretManager : TSingleton<TurretManager>
{
    private Dictionary<int, Turret> _OccupiedDegrees = new Dictionary<int, Turret>();
    //private Dictionary<int, Turret> _OccupiedDegrees = new Dictionary<int, TurretCSV>();

    public List<Turret> GetAllTurrets()
    {
        List<Turret> list = new List<Turret>();
        foreach (var item in _OccupiedDegrees)
        {
            list.Add(item.Value);
        }
        return list;
    }

    public Turret GetTurret(int degree)
    {
        if (!_OccupiedDegrees.ContainsKey(degree))
        {
            return null;
        }
        return _OccupiedDegrees[degree];
    }

    public void AddTurret(int degree, int turretId, GameObject pivot, float radius)
    {
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(turretId.ToString());
        if (csv == null)
        {
            Debug.LogError("CreateCannon csv is empty! ");
            return;
        }

        // check position
        degree %= 360;
        if (_OccupiedDegrees.ContainsKey(degree))
        {
            var v = UIManager.Instance.Open<MessageView>();
            v.SetData("位置已被占用,建造失败");
            Debug.LogFormat("degree {0} has been occupied! ", degree);
            Debugger.Log(string.Format("degree {0} has been occupied! ", degree));
            return;
        }

        // check golds
        int cost = csv._Price;
        if (ArchiveManager.instance.GetGoldCount() < cost)
        {
            var v = UIManager.Instance.Open<MessageView>();
            v.SetData("金币不足,建造失败");
            return;
        }

        // check materials
        bool isStockSufficient = CheckMaterials(csv._Materials);
        if (!isStockSufficient)
        {
            return;
        }

        // create game object
        var c = GameObject.Instantiate(pivot);
        c.name = "Pivot_" + degree;
        c.transform.SetParent(PlanetController.instance.transform);
        c.SetActive(true);
        c.transform.localScale = Vector3.one;
        Turret turret = ResourcesManager.instance.GetTurret(GameConfig.instance._TurretPrefabName);
        if (turret == null)
        {
            Debugger.Log(string.Format("turret is empty!"));
            return;
        }
        turret.transform.SetParent(c.transform);
        turret.gameObject.SetActive(true);
        turret.transform.localScale = Vector3.one;
        turret.transform.localPosition = new Vector3(0, radius + GameConfig.instance._cannonHalfHeight_Common, 0);
        turret.transform.localRotation = Quaternion.identity;
        turret.SetData(degree, EFaction.Ours, turretId);
        c.transform.localEulerAngles = new Vector3(0, 0, degree);
        Debugger.Log("degree=" + degree);

        // 添加指定位置炮塔
        turret._onDie = RemoveTurret;
        if (_OccupiedDegrees.ContainsValue(turret))
        {
            Debugger.Log(string.Format("turret {0} has been add twice! ", turret.name));
            return;
        }
        _OccupiedDegrees.Add(degree, turret);

        // 建造成功扣除金币
        EventDispatcher.instance.DispatchEvent(EventID.AddGold, -cost);
        EventDispatcher.instance.DispatchEvent(EventID.CreateTurretSuccess, degree, turretId);
    }

    bool CheckMaterials(Dictionary<int, int> materials)
    {
        foreach (var item in materials)
        {
            if (ArchiveManager.instance.GetMaterialCount(item.Key) < item.Value)
            {
                var v = UIManager.Instance.Open<MessageView>();
                v.SetData("材料不足,建造失败");
                return false;
            }
        }
        ArchiveManager.instance.ConsumeMaterials(materials);
        return true;
    }

    public int GetTurretDegree(Turret turret)
    {
        int degree = -1;
        if (turret == null)
        {
            Debugger.Log(string.Format("turret is empty!"));
            return degree;
        }

        foreach (var item in _OccupiedDegrees)
        {
            if (item.Value == turret)
            {
                degree = item.Key;
                break;
            }
        }
        return degree;
    }

    public void RemoveTurret(int degree)
    {
        if (degree >= 0 && _OccupiedDegrees.ContainsKey(degree))
        {
            var turret = _OccupiedDegrees[degree];
            GameObject.Destroy(turret.gameObject);
            _OccupiedDegrees.Remove(degree);
        }
    }

    public override void Clear()
    {
        _OccupiedDegrees.Clear();
        base.Clear();
    }
}
