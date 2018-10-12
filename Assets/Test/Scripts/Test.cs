using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Test : MonoBehaviour
{
    [SerializeField] Image _Image;
    UniqueIDGenerator _UniqueIDGenerator = new UniqueIDGenerator();
    Dictionary<uint, IEnumerator> _RoutineDict = new Dictionary<uint, IEnumerator>();

    void Start()
    {
        float maxTime = 3; 
        CallEveryFrameInAPeriod(maxTime, (time) => _Image.material.SetFloat("_Ratio", time / maxTime));
    }

    private void Update()
    {
        //var t = _Image.material.GetFloat("_CountDownTime");
        //Debugger.LogTealBold(t);
    }

    public uint CallEveryFrameInAPeriod(float time, Action<float> callEveryFrame, Action onFinish = null)
    {
        uint id = _UniqueIDGenerator.GetUniqueID();
        onFinish += () =>
        {
            if (_RoutineDict.ContainsKey(id))
            {
                StopCoroutine(_RoutineDict[id]);
                _RoutineDict.Remove(id);
            }
        };
        var routine = CallEveryFrameRoutine(time, callEveryFrame, onFinish);
        _RoutineDict.Add(id, routine);
        StartCoroutine(routine);
        return id;
    }

    public void CancelCallEveryFrameInAPeriod(uint id)
    {
        if (_RoutineDict.ContainsKey(id))
        {
            StopCoroutine(_RoutineDict[id]);
            _RoutineDict.Remove(id);
        }
    }

    IEnumerator CallEveryFrameRoutine(float expireTime, Action<float> callEveryFrame, Action onFinish)
    {
        float time = 0;
        while (time < expireTime)
        {
            yield return null;
            time += Time.deltaTime;
            if (callEveryFrame != null)
            {
                callEveryFrame(time);
            }
        }
        if (onFinish != null)
        {
            onFinish();
        }
    }
}