using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIFramwork;

public class Planet : MonoBehaviour
{
    [SerializeField] protected GameObject _cannonPivotPrefab;

    [SerializeField] protected float _radius = 2.5f;
    public float radius { get { return _radius; } }

    [SerializeField] protected BaseRotate _rotate;
    public BaseRotate rotate { get { return _rotate; } }

    [SerializeField] protected int _minCannonGap = 60;
    public int minCannonGap { get { return _minCannonGap; } }

    [SerializeField] protected float _visualField = 10;
    public float visualField { get { return _visualField; } }

    [SerializeField] Text _hpText;
    int _hp;
    public int hp
    {
        set
        {
            _hpText.text = value + "/" + _maxHP;
            _hp = value;
            if (hp <= 0)
            {
                var v = ViewManager.instance.Open<EndView>();
                v.SetData("Mission Failed! ");
                EventDispatcher.instance.DispatchEvent(EventID.End);
            }
        }
        get
        {
            return _hp;
        }
    }
    public int _maxHP = 10;
    public int _defense = 2;

    public Dictionary<int, GameObject> cannonPivotDict { get; private set; }
    protected List<int> _allDegrees = new List<int>();

    public void Awake()
    {
        cannonPivotDict = new Dictionary<int, GameObject>();

        // 计算所有能建造的炮塔位置
        int curDegree = 0;
        while (curDegree < 360)
        {
            _allDegrees.Add(curDegree);
            curDegree += _minCannonGap;
        }

        Init();
    }

    public void Init()
    {
        hp = _maxHP;
        transform.localEulerAngles = Vector3.zero;
        foreach (var item in cannonPivotDict)
        {
            GameObject.Destroy(item.Value.gameObject);
        }
        cannonPivotDict.Clear();
    }

    public void CreateCannon(int degree)
    {
        // 金币不足,建造失败
        int cost = 20;
        if (GameData.instance.goldCount < cost)
        {
            var v = ViewManager.instance.Open<TipsView>();
            v.SetData("金币不足,建造失败");
            return;
        }

        degree %= 360;
        // 检查字典中是否已经添加对应位置炮塔
        // 位置已被占用,建造失败
        if (cannonPivotDict.ContainsKey(degree))
        {
            var v = ViewManager.instance.Open<TipsView>();
            v.SetData("位置已被占用,建造失败");
            Debug.LogFormat("degree {0} has been occupied! ", degree);
            return;
        }

        // 建造成功扣除金币
        EventDispatcher.instance.DispatchEvent(EventID.AddGold, -cost);
        EventDispatcher.instance.DispatchEvent(EventID.CreateTurretSuccess, degree);

        var c = GameObject.Instantiate(_cannonPivotPrefab);
        c.name = "Pivot_" + degree;
        c.transform.SetParent(transform);
        c.SetActive(true);
        c.transform.localScale = Vector3.one;
        var a = GameObject.Instantiate(GameAssets.instance._cannonPrefab);
        a.transform.SetParent(c.transform);
        a.SetActive(true);
        a.transform.localScale = Vector3.one;
        a.transform.localPosition = new Vector3(0, radius + GameConfig.instance._cannonHalfHeight_Common, 0);
        a.transform.localRotation = Quaternion.identity;
        a.GetComponent<Cannon>()._degree = degree;
        c.transform.localEulerAngles = new Vector3(0, 0, degree);
        Debug.Log("degree=" + degree);

        // 添加指定位置炮塔
        var comp = a.GetComponent<Cannon>();
        comp._onDie = OnRemoveCannon;
        cannonPivotDict.Add(degree, c);
    }

    void OnRemoveCannon(int degree)
    {
        if (cannonPivotDict.ContainsKey(degree))
        {
            cannonPivotDict.Remove(degree);
        }
    }

    public void CreatCannonQuick()
    {
        CreateCannon(FindCannonToBeBuiltPos());
    }

    // 炮塔建造策略
    int FindCannonToBeBuiltPos()
    {
        List<int> emptyDegrees = new List<int>(_allDegrees);
        List<int> occupiedDegrees = new List<int>();
        foreach (var p in cannonPivotDict)
        {
            var k = p.Key;
            if (!emptyDegrees.Contains(k))
            {
                Debug.LogErrorFormat("emptyDegrees does not contains degree {0}! ", k);
                continue;
            }
            occupiedDegrees.Add(k);
            // 由于要查找下一个已建造点,因此排序
            occupiedDegrees.Sort();
            emptyDegrees.Remove(k);
        }

        if (occupiedDegrees.Count == 0)
        {
            return 0;
        }

        // 查找相距最大的两个点
        int minDegree = 0;
        int maxDegree = 0;

        // 将最小的点增加360再加入列表
        // 为了计算这个点0度和这个点360度的中点距离位置
        occupiedDegrees.Add(occupiedDegrees[0] + 360);
        //if (occupiedDegrees.Count == 1)
        //{
        //    minDegree = 0;
        //    maxDegree = 360;
        //}
        //else
        //{
        minDegree = occupiedDegrees[0];
        maxDegree = occupiedDegrees[1];
        int gap = 0;
        for (int i = 0, length = occupiedDegrees.Count - 1; i < length; i++)
        {
            int newGap = occupiedDegrees[i + 1] - occupiedDegrees[i];
            if (newGap < 0)
            {
                Debug.LogErrorFormat("occupiedDegrees delta is minus! degree1={0}, degree0={1}", occupiedDegrees[i + 1], occupiedDegrees[i]);
                continue;
            }
            if (gap < newGap)
            {
                minDegree = occupiedDegrees[i];
                maxDegree = occupiedDegrees[i + 1];
                gap = newGap;
            }
        }
        //}

        // 查找empty表中距离中点最近的点
        int middleDegree = (minDegree + maxDegree) / 2;
        int targetDegree = 0;
        int emptyGap = 360;
        for (int i = 0, length = emptyDegrees.Count; i < length; i++)
        {
            var e = emptyDegrees[i];
            int newEmptyGap = Mathf.Abs(middleDegree - e);
            if (emptyGap > newEmptyGap)
            {
                emptyGap = newEmptyGap;
                targetDegree = e;
            }
        }
        return targetDegree;
    }

    public bool IsInVisualField(Vector3 pos)
    {
        float distance = Vector3.Distance(pos, transform.position);
        return distance <= visualField;
    }

    void OnTriggerEnter(Collider collider)
    {
        ExecuteAttack(collider.gameObject.GetComponent<Rock>());
    }

    void ExecuteAttack(Rock rock)
    {
        if (rock == null)
        {
            return;
        }

        hp -= BattleUtil.CalcDamage(rock._attack, _defense);
    }
}
