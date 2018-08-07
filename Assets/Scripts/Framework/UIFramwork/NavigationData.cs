using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    enum EUILayer
    {
        FullScreen = 0,
        Resident = 100,
        Popup = 200,
        Tips = 300,
    }

    enum EUIType
    {
        FullScreen,
        Resident,
        Coexisting,
        Independent
    }

    [System.Serializable]
    class NavigationData
    {
        public EUILayer _Layer;
        public bool _CloseByDestroy; 
        public EUIType _Type; 
        //public uint _Group;
        //public bool _IsJumpBack; // 向回跳转，弹出之后的所有UI
        //public bool _IsMultiple;

        // 隐藏FullScreenUI之后，关闭共存UI，而不仅仅是隐藏
        public bool _IsCloseCoexistingUI = true;

        // 一些额外参数，比如是否打开某些ResidentUI上的元素
        public List<object> _UIParams = new List<object>(); 
    }
}
