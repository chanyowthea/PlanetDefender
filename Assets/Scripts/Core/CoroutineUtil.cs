using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions.Comparers;

public class CoroutineUtil : MonoSingleton<CoroutineUtil>
{
	public static IEnumerator WaitThenDo(float waitTime, Action action)
	{
		var coroutine = WaitThenDoRoutine(waitTime, action);
		instance.StartCoroutine(coroutine);
		return coroutine;
	}
	public static void Start(IEnumerator routine)
	{
		if (routine == null) { return; }
		instance.StartCoroutine(routine);
	}
	public static void Stop(IEnumerator routine)
	{
		if (routine == null) { return; }
		instance.StopCoroutine(routine);
	}
	public static void Start(IEnumerator routine, Action onFinish)
	{
		if (routine == null)
		{
			onFinish();
			return;
		}
		instance.StartCoroutine(DoRoutine(routine, onFinish));
	}
	private static IEnumerator DoRoutine(IEnumerator routine, Action onFinish)
	{
		yield return instance.StartCoroutine(routine);
		if (onFinish != null)
		{
			onFinish(); 
		}
	}
	private static IEnumerator WaitThenDoRoutine(float waitTime, Action action)
	{
		yield return Yielders.GetWaitForSeconds(waitTime);
		if (action != null)
		{
			action(); 
		}
	}
}

public static class Yielders
{
	public static bool Enabled = true;  

	public static int _internalCounter = 0;

	static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
	public static WaitForEndOfFrame EndOfFrame
	{
		get { _internalCounter++; return Enabled ? _endOfFrame : new WaitForEndOfFrame(); }
	}

	static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
	public static WaitForFixedUpdate FixedUpdate
	{
		get { _internalCounter++; return Enabled ? _fixedUpdate : new WaitForFixedUpdate(); }
	}

	public static WaitForSeconds GetWaitForSeconds(float seconds)
	{
		_internalCounter++; 

		if (!Enabled)
			return new WaitForSeconds(seconds);

		WaitForSeconds wfs;
		if (!_waitForSecondsYielders.TryGetValue(seconds, out wfs))
			_waitForSecondsYielders.Add(seconds, wfs = new WaitForSeconds(seconds));
		return wfs;
	}

	public static void ClearWaitForSeconds()
	{
		_waitForSecondsYielders.Clear();
	}

	static Dictionary<float, WaitForSeconds> _waitForSecondsYielders = new Dictionary<float, WaitForSeconds>(100, new FloatComparer());
}

