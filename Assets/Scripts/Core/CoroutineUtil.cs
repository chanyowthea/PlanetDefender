using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Comparers;

public class CoroutineUtil : MonoSingleton<CoroutineUtil>
{
    public IEnumerator Wait(float time, Action onFinish, bool isRealTime = false)
    {
        var r = WaitRoutine(time, onFinish, isRealTime);
        StartCoroutine(r);
        return r;
    }

    IEnumerator WaitRoutine(float waitTime, Action action, bool isRealTime = false)
    {
        if (isRealTime)
        {
            yield return new WaitForSecondsRealtime(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(waitTime);
        }
        if (action != null)
        {
            action();
        }
    }
}