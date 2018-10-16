using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UIFramework;

public class BuildSlot : MonoBehaviour
{
    public bool HasBuild { private set; get; }
    public int Degree { private set; get; }
    [SerializeField] Image _Image;
    [SerializeField] GameObject _UnBuildStateObj;
    Action<int> _OnBuild;

    public void SetData(int degree, Action<int> onBuild)
    {
        Degree = degree;
        _OnBuild = onBuild;
    }

    public void OnClickDismantle()
    {
        var ui = UIManager.Instance.Open<PromptView>();
        ui.SetData("Are you sure to dismantle this turret?", () =>
        {
            var turret = TurretManager.instance.GetTurret(Degree);
            if (turret != null)
            {
                TurretManager.instance.RemoveTurret(Degree);
                var view = UIManager.Instance.Open<MessageView>();
                view.SetData("Dismantled successfully. ");
                OnBuild(0, false);
            }
        });
    }

    public void OnClickBuild()
    {
        if (_OnBuild != null)
        {
            _OnBuild(Degree);
        }
    }

    public void ClearData()
    {
        ResourcesManager.instance.UnloadAsset(_Image.sprite);
        _Image.sprite = null;
    }

    public void OnBuild(int turrectId, bool isBuild = true)
    {
        HasBuild = isBuild;
        _UnBuildStateObj.SetActive(!isBuild);
        _Image.gameObject.SetActive(isBuild);
        if (turrectId != 0)
        {
            var csv = ConfigDataManager.instance.GetData<TurretCSV>(turrectId.ToString());
            if (csv == null)
            {
                Debug.LogError("CreateCannon csv is empty! ");
                return;
            }
            _Image.sprite = ResourcesManager.instance.GetSprite(csv._Picture);
            _Image.rectTransform.sizeDelta = ImageUtil.ResizeSprite(_Image.sprite.rect.width / _Image.sprite.rect.height,
                _Image.rectTransform.rect, EScaleType.Expand);
        }
    }
}
