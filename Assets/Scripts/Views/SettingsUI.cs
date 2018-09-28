using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;
using UnityEngine.UI;
using System;

class SettingsUI : BaseUI
{
    [SerializeField] Dropdown _DropDown;
    public SettingsUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.Popup;
    }

    public override void Open(NavigationData data = null)
    {
        base.Open(data);

        _DropDown.ClearOptions();
        _DropDown.AddOptions(LocManager.instance.GetSupportLanguagesLoc());
        _DropDown.onValueChanged.AddListener(OnValueChanged);
        // TODO
        CoroutineUtil.instance.Wait(Time.deltaTime * 2, () => ResetDropDownLanguage());
    }

    internal override void Close()
    {
        _DropDown.onValueChanged.RemoveListener(OnValueChanged);
        base.Close();
    }

    void ResetDropDownLanguage()
    {
        var children = _DropDown.transform.GetComponentsInChildren<LocComponent>();
        var lans = LocManager.instance.GetSupportLanguagesID();
        for (int i = 0, length = children.Length; i < length; i++)
        {
            children[i].StringID = lans[i];
        }
    }

    void OnValueChanged(int index)
    {
        LocLang lang = (LocLang)index;
        LocManager.instance.CurrentLanguage = lang;
        ResetDropDownLanguage();
    }

    public void OnClickExit()
    {
        var view = UIManager.Instance.Open<PromptView>();
        view.SetData("Back to start view?", () =>
        {
            EventDispatcher.instance.DispatchEvent(EventID.End);
            Facade.instance.ChangeScene(GameConfig.instance._LauncherSceneName);
        });
    }

    public void OnClickClose()
    {
        UIManager.Instance.Close(this);
    }
}
