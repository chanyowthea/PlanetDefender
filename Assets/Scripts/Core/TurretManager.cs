using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;

public class TurretManager : TSingleton<TurretManager>
{
    private Dictionary<int, Turret> _OccupiedDegrees = new Dictionary<int, Turret>();

    public List<Turret> GetAllTurrets()
    {
        List<Turret> list = new List<Turret>(); 
        foreach (var item in _OccupiedDegrees)
        {
            list.Add(item.Value); 
        }
        return list;
    }

    public void AddTurret(int degree, int turretId, GameObject pivot, float radius)
    {
        var csv = ConfigDataManager.instance.GetData<TurretCSV>(turretId.ToString());
        if (csv == null)
        {
            Debug.LogError("CreateCannon csv is empty! ");
            return;
        }

        int cost = csv._Price;
        if (ArchiveManager.instance.GetGoldCount() < cost)
        {
            var v = UIManager.Instance.Open<MessageView>();
            v.SetData("金币不足,建造失败");
            return;
        }

        //int cost = csv._Materials;
        //if (ArchiveManager.instance.GetGoldCount() < cost)
        //{
        //    var v = UIManager.Instance.Open<MessageView>();
        //    v.SetData("金币不足,建造失败");
        //    return;
        //}

        degree %= 360;
        if (_OccupiedDegrees.ContainsKey(degree))
        {
            var v = UIManager.Instance.Open<MessageView>();
            v.SetData("位置已被占用,建造失败");
            Debug.LogFormat("degree {0} has been occupied! ", degree);
            DebugFramework.Debugger.Log(string.Format("degree {0} has been occupied! ", degree));
            return;
        }
        var c = GameObject.Instantiate(pivot);
        c.name = "Pivot_" + degree;
        c.transform.SetParent(PlanetController.instance.transform);
        c.SetActive(true);
        c.transform.localScale = Vector3.one;
        var a = GameObject.Instantiate(GameAssets.instance._cannonPrefab);
        a.transform.SetParent(c.transform);
        a.SetActive(true);
        a.transform.localScale = Vector3.one;
        a.transform.localPosition = new Vector3(0, radius + GameConfig.instance._cannonHalfHeight_Common, 0);
        a.transform.localRotation = Quaternion.identity;
        
        Turret turret = a.GetComponent<Turret>();
        if (turret == null)
        {
            DebugFramework.Debugger.Log(string.Format("turret is empty!"));
            return;
        }

        turret.SetData(degree, EFaction.Ours, turretId);
        c.transform.localEulerAngles = new Vector3(0, 0, degree);
        DebugFramework.Debugger.Log("degree=" + degree);

        // 添加指定位置炮塔
        turret._onDie = RemoveTurret;
        if (_OccupiedDegrees.ContainsValue(turret))
        {
            DebugFramework.Debugger.Log(string.Format("turret {0} has been add twice! ", turret.name));
            return;
        }
        _OccupiedDegrees.Add(degree, turret);

        // 建造成功扣除金币
        EventDispatcher.instance.DispatchEvent(EventID.AddGold, -cost);
        EventDispatcher.instance.DispatchEvent(EventID.CreateTurretSuccess, degree, turretId);

    }

    public int GetTurretDegree(Turret turret)
    {
        int degree = -1;
        if (turret == null)
        {
            DebugFramework.Debugger.Log(string.Format("turret is empty!"));
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
        if (degree >= 0)
        {
            _OccupiedDegrees.Remove(degree);
        }
    }
}
