using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramework;

public class Planet : Army
{
    [SerializeField] protected GameObject _cannonPivotPrefab;

    [SerializeField] protected float _radius = 2.5f;
    public float radius { get { return _radius; } }

    [SerializeField] protected BaseRotate _rotate;
    public BaseRotate rotate { get { return _rotate; } }

    [SerializeField] float _VisualField;
    public override float VisualField
    {
        get
        {
            return _VisualField;
        }
    }

    protected List<int> _allDegrees = new List<int>();
    [SerializeField] Text _hpText;
    [SerializeField] GameObject _PoisionPivot;

    public override float HP
    {
        set
        {
            if (GameConfig.instance._PlanetImmuneDamage)
            {
                return;
            }
            _hpText.text = Mathf.CeilToInt(value) + "/" + MaxHP;
            base.HP = value;
            if (HP <= 0)
            {
                var v = UIManager.Instance.Open<EndView>();
                v.SetData("Mission Failed! ");
                EventDispatcher.instance.DispatchEvent(EventID.End);
            }
        }
        get
        {
            return base.HP;
        }
    }
    bool _IsInPoisionState;

    public override void Init()
    {
        base.Init();
        Faction = EFaction.Ours;

        var csv = ConfigDataManager.instance.GetData<PlayerCSV>(ArchiveManager.instance.GetCurrentLevel().ToString());
        if (csv != null)
        {
            MaxHP = csv._MaxHP;
            HP = MaxHP;
            _Defense = csv._Defense;
        }
        transform.localEulerAngles = Vector3.zero;
        _PoisionPivot.SetActive(_IsInPoisionState);
    }

    public void DoHurt(int enemyId)
    {
        var csv = ConfigDataManager.instance.GetData<EnemyCSV>(enemyId.ToString());
        if (csv == null)
        {
            Debugger.LogError("csv is empty! ");
            return;
        }

        int hurt = BattleUtil.CalcDamage(csv._Attack, _Defense);
        DoHurtValue(hurt);
        if (hurt > 0)
        {
            if (csv._Type == EEnemyType.Poision && !_IsInPoisionState)
            {
                _IsInPoisionState = true;
                _PoisionPivot.SetActive(true);
                GameManager.instance.DelayCall(3, () => DoHurtValue(hurt), true);
            }
        }
    }

    public void DoHurtValue(float value)
    {
        if (value < 0)
        {
            return;
        }

        HP -= value;
    }

    public void CreateCannon(int degree, int turrectId)
    {
        TurretManager.instance.AddTurret(degree, turrectId, _cannonPivotPrefab, radius);
    }

    public GameObject GetTurretPivot()
    {
        return _cannonPivotPrefab;
    }

    //public void CreatCannonQuick()
    //{
    //    CreateCannon(FindCannonToBeBuiltPos(), 1);
    //}

    //// 炮塔建造策略
    //int FindCannonToBeBuiltPos()
    //{
    //    List<int> emptyDegrees = new List<int>(_allDegrees);
    //    List<int> occupiedDegrees = new List<int>();
    //    foreach (var p in cannonPivotDict)
    //    {
    //        var k = p.Key;
    //        if (!emptyDegrees.Contains(k))
    //        {
    //            Debug.LogErrorFormat("emptyDegrees does not contains degree {0}! ", k);
    //            continue;
    //        }
    //        occupiedDegrees.Add(k);
    //        // 由于要查找下一个已建造点,因此排序
    //        occupiedDegrees.Sort();
    //        emptyDegrees.Remove(k);
    //    }

    //    if (occupiedDegrees.Count == 0)
    //    {
    //        return 0;
    //    }

    //    // 查找相距最大的两个点
    //    int minDegree = 0;
    //    int maxDegree = 0;

    //    // 将最小的点增加360再加入列表
    //    // 为了计算这个点0度和这个点360度的中点距离位置
    //    occupiedDegrees.Add(occupiedDegrees[0] + 360);
    //    minDegree = occupiedDegrees[0];
    //    maxDegree = occupiedDegrees[1];
    //    int gap = 0;
    //    for (int i = 0, length = occupiedDegrees.Count - 1; i < length; i++)
    //    {
    //        int newGap = occupiedDegrees[i + 1] - occupiedDegrees[i];
    //        if (newGap < 0)
    //        {
    //            Debug.LogErrorFormat("occupiedDegrees delta is minus! degree1={0}, degree0={1}", occupiedDegrees[i + 1], occupiedDegrees[i]);
    //            continue;
    //        }
    //        if (gap < newGap)
    //        {
    //            minDegree = occupiedDegrees[i];
    //            maxDegree = occupiedDegrees[i + 1];
    //            gap = newGap;
    //        }
    //    }

    //    // 查找empty表中距离中点最近的点
    //    int middleDegree = (minDegree + maxDegree) / 2;
    //    int targetDegree = 0;
    //    int emptyGap = 360;
    //    for (int i = 0, length = emptyDegrees.Count; i < length; i++)
    //    {
    //        var e = emptyDegrees[i];
    //        int newEmptyGap = Mathf.Abs(middleDegree - e);
    //        if (emptyGap > newEmptyGap)
    //        {
    //            emptyGap = newEmptyGap;
    //            targetDegree = e;
    //        }
    //    }
    //    return targetDegree;
    //}

    public bool IsInVisualField(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, transform.position);
        return distance <= VisualField;
    }

    void OnTriggerEnter(Collider collider)
    {
        // need to use a universal method
        ExecuteAttack(collider.gameObject.GetComponent<Enemy>());
        var gold = collider.gameObject.GetComponent<Gold>();
        if (gold != null)
        {
            EventDispatcher.instance.DispatchEvent(EventID.AddGold, (int)gold.HP);
        }
    }

    void ExecuteAttack(Enemy rock)
    {
        if (rock == null)
        {
            return;
        }
        Debug.Log("ExecuteAttack value=" + BattleUtil.CalcDamage(rock.Attack, _Defense));
        //EventDispatcher.instance.DispatchEvent(EventID.AddHealth, -BattleUtil.CalcDamage(rock.Attack, _Defense));
        DoHurt(rock.EnemyID);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, VisualField);
    }
#endif
}
