using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageUtil
{
    public static Vector2 ResizeSprite(float spriteWHRatio, Rect rect, EScaleType scaleType = EScaleType.Shrink)
    {
        // for the layout component affects the rect transform value. 
        Canvas.ForceUpdateCanvases();
        var spriteRect = rect.size;
        float maskRatio = rect.width / rect.height; ;

        // the ratio of sprite
        float ratio = spriteWHRatio;
        float w = 0;
        float h = 0;
        if (scaleType == EScaleType.Shrink)
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
        else if (scaleType == EScaleType.Expand)
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
        return new Vector2(w, h);
    }
}
