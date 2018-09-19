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

    [SerializeField] protected int _minCannonGap = 60;
    public int minCannonGap { get { return _minCannonGap; } }

    [SerializeField] float _VisualField;
    public override float VisualField
    {
        get
        {
            return _VisualField;
        }
    }

    //public Dictionary<int, GameObject> cannonPivotDict { get; private set; }
    protected List<int> _allDegrees = new List<int>();
    [SerializeField] Text _hpText;

    public override int HP
    {
        set
        {
            _hpText.text = value + "/" + MaxHP;
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
    public void Awake()
    {
        //cannonPivotDict = new Dictionary<int, GameObject>();

        // 计算所有能建造的炮塔位置
        int curDegree = 0;
        while (curDegree < 360)
        {
            _allDegrees.Add(curDegree);
            curDegree += _minCannonGap;
        }

        Init();
    }

    public override void Init()
    {
        base.Init();
        Faction = EFaction.Ours;

        // TODO 使用表加载
        MaxHP = 10;
        HP = MaxHP;
        transform.localEulerAngles = Vector3.zero;
        //foreach (var item in cannonPivotDict)
        //{
        //    GameObject.Destroy(item.Value.gameObject);
        //}
        //cannonPivotDict.Clear();
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
        ExecuteAttack(collider.gameObject.GetComponent<Enemy>());
    }

    void ExecuteAttack(Enemy rock)
    {
        if (rock == null)
        {
            return;
        }
        Debug.Log("ExecuteAttack value=" + BattleUtil.CalcDamage(rock.Attack, _Defense)); 
        EventDispatcher.instance.DispatchEvent(EventID.AddHealth, -BattleUtil.CalcDamage(rock.Attack, _Defense));
    }
}
