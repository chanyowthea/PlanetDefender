using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUtil
{
    static public void MoveRect(RectTransform rect, float x, float y)
    {
        int ix = Mathf.FloorToInt(x + 0.5f);
        int iy = Mathf.FloorToInt(y + 0.5f);

        Transform t = rect.transform;
        t.localPosition += new Vector3(ix, iy);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(rect);
#endif
    }
}
