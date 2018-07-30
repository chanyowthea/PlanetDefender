using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramwork
{
    public enum ELayer
    {
        None,
        FullScreen = 1000,
        Window = 2000,
        Top = 3000,
    }

    public enum ECloseType
    {
        Hide,
        Destroy
    }

    public enum EOpenType
    {
        None, 
        HideOther
    }
}
