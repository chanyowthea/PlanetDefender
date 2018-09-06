using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class LoadingView : BaseUI
{
    private AsyncOperation _Operation;
    public Slider _Slider;
    public Text _Label;

    public LoadingView()
    {
        _NaviData._Type = EUIType.FullScreen;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = true;
    }

    public void SetData(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            return;
        }
        StartCoroutine(StartLoadScene(sceneName));
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
