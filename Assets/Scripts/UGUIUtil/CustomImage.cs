using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EScaleType
{
    Shrink,
    Expand
}

public class CustomImage : MonoBehaviour
{
    [SerializeField] RectMask2D _Mask;
    [SerializeField] Image _Image;
    [SerializeField] EScaleType _ScaleType;

    public Sprite sprite
    {
        get { return _Image.sprite; }
        set { SetData(value); }
    }

    public Vector2 ImageSize
    {
        get
        {
            return _Image.rectTransform.sizeDelta;
        }
    }

    public void SetData(Sprite s)
    {
        if (s == null)
        {
            _Image.sprite = null;
            return;
        }
        // for the layout component affects the rect transform value. 
        Canvas.ForceUpdateCanvases();
        var spriteRect = _Mask.rectTransform.rect.size;
        float maskRatio = _Mask.rectTransform.rect.width / _Mask.rectTransform.rect.height; ;

        // the ratio of sprite
        float ratio = s.rect.width / s.rect.height;
        float w = 0;
        float h = 0;
        if (_ScaleType == EScaleType.Shrink)
        {
            if (ratio > maskRatio)
            {
                w = spriteRect.x;
                h = spriteRect.x / ratio;
            }
            else
            {
                w = spriteRect.y * ratio;
                h = spriteRect.y;
            }
        }
        else if (_ScaleType == EScaleType.Expand)
        {
            if (ratio <= maskRatio)
            {
                w = spriteRect.x;
                h = spriteRect.x / ratio;
            }
            else
            {
                w = spriteRect.y * ratio;
                h = spriteRect.y;
            }
        }
        _Image.rectTransform.sizeDelta = new Vector2(w, h);
        _Image.sprite = s;
    }
}
