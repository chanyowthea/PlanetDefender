using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ERoutinePlace
{
    UI,
    InGame,
}

public class CoroutineUtil : MonoSingleton<CoroutineUtil>
{
    Dictionary<ERoutinePlace, List<IEnumerator>> _WaitRoutines = new Dictionary<ERoutinePlace, List<IEnumerator>>()
    {
        { ERoutinePlace.UI, new List<IEnumerator>()},
        { ERoutinePlace.InGame, new List<IEnumerator>()}
    };
    public IEnumerator Wait(float time, Action onFinish, ERoutinePlace eRoutinePlace = ERoutinePlace.InGame)
    {
        var r = WaitRoutine(time, onFinish);
        if (_WaitRoutines.ContainsKey(eRoutinePlace))
        {
            var list = _WaitRoutines[eRoutinePlace];
            list.Add(r);
        }
        StartCoroutine(r);
        return r;
    }

    public void ClearRoutines(ERoutinePlace eRoutinePlace = ERoutinePlace.InGame)
    {
        if (!_WaitRoutines.ContainsKey(eRoutinePlace))
        {
            return;
        }

        var list = _WaitRoutines[eRoutinePlace];
        for (int i = 0, length = list.Count; i < length; i++)
        {
            var r = list[i];
            if (r != null)
            {
                StopCoroutine(r);
            }
        }
        list.Clear();
    }

    IEnumerator WaitRoutine(float waitTime, Action action, ERoutinePlace eRoutinePlace = ERoutinePlace.InGame)
    {
        float time = 0;
        while (time < waitTime)
        {
            yield return null;
            time += eRoutinePlace != ERoutinePlace.InGame ? 
                Facade.instance._UITimer.DeltaTime : GameManager.instance._Timer.DeltaTime;
        }

        if (action != null)
        {
            action();
        }
    }
}