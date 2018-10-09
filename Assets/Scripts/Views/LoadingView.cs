using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

class LoadingView : BaseUI
{
    private AsyncOperation _Operation;
    Action<string> _OnSceneLoaded;
    public Slider _Slider;
    public Text _Label;
    string _SceneName;

    public LoadingView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public void SetData(string sceneName, Action<string> onSceneLoaded = null)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }
        if (onSceneLoaded != null)
        {
            _OnSceneLoaded += onSceneLoaded;
        }
        _SceneName = sceneName;
        StartCoroutine(StartLoadScene(sceneName));
    }

    internal override void ClearData()
    {
        _OnSceneLoaded = null;
        base.ClearData();
    }

    IEnumerator StartLoadScene(string sceneName)
    {
        _Operation = SceneManager.LoadSceneAsync(sceneName);
        _Operation.allowSceneActivation = false;
        yield return null;
    }

    float mActualProcess = 0;
    float mCurProcess = 0;

    void Update()
    {
        if (_Operation != null)
        {
            mActualProcess = _Operation.progress;
            if (_Operation.progress >= 0.9f)
                mActualProcess = 1;

            if (mCurProcess < mActualProcess)
            {
                mCurProcess += 0.01f;
            }
            mCurProcess = Mathf.Clamp(mCurProcess, 0, mActualProcess);

            ProcessValue = mCurProcess;

            if (mCurProcess == 1)
            {
                if (_OnSceneLoaded != null)
                {
                    _OnSceneLoaded(_SceneName);
                }
                _Operation.allowSceneActivation = true;
            }
        }
    }

    float ProcessValue
    {
        set
        {
            _Slider.value = value;
            _Label.text = string.Format("{0}%", Mathf.CeilToInt(value * 100));
        }
    }
}
