using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UIFramework
{
    // 关闭就是清除数据， 隐藏不清除数据
    // 层级设置
    class UIManager
    {
        static UIManager _Instance;
        public static UIManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new UIManager();
                }
                return _Instance;
            }
        }

        // 所有UI的对象池
        Dictionary<Type, Queue<BaseUI>> _UIPool = new Dictionary<Type, Queue<BaseUI>>();

        // 常驻UI例如返回键，金币，好友按钮
        List<BaseUI> _ResidentUI = new List<BaseUI>();

        // 已经打开的全屏界面
        // List作为栈，但也可从中间删除
        List<BaseUI> _OpenedFullScreenUI = new List<BaseUI>();

        // 共存的UI，例如物品详情Tips，弹窗等
        // List作为栈，但也可从中间删除
        Dictionary<BaseUI, List<BaseUI>> _CoexistingUI = new Dictionary<BaseUI, List<BaseUI>>();

        // 独立的UI，例如滚屏Tips，网络请求或加载中等
        List<BaseUI> _IndependentUI = new List<BaseUI>();

        public BaseUI CurFullScreenUI
        {
            get
            {
                if (_OpenedFullScreenUI.Count == 0)
                {
                    return null;
                }
                return _OpenedFullScreenUI[_OpenedFullScreenUI.Count - 1];
            }
        }

        static Transform _UIParent;
        static Dictionary<EUILayer, Transform> _UILayers = new Dictionary<EUILayer, Transform>();

        // 几种特殊情况
        // 连续几个弹窗依次出现
        // 同时出现两个弹窗
        // 

        public UIManager()
        {
            InitUIParent();
        }

        void InitUIParent()
        {
            _UIParent = new GameObject("UIParent").transform;
            UnityEngine.Object.DontDestroyOnLoad(_UIParent);
            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            eventSystem.transform.SetParent(_UIParent);

            var ns = Enum.GetNames(typeof(EUILayer));
            for (int i = 0, length = ns.Length; i < length; i++)
            {
                var n = ns[i];
                EUILayer l = (EUILayer)Enum.Parse(typeof(EUILayer), n);
                if (!_UILayers.ContainsKey(l))
                {
                    var tf = new GameObject(n).transform;
                    tf.SetParent(_UIParent);
                    _UILayers.Add(l, tf);
                }
            }
        }


        T Get<T>()
            where T : BaseUI
        {
            var t = typeof(T);
            Debug.Log("Get t=" + t);
            if (_UIPool.ContainsKey(t))
            {
                var dict = _UIPool[t];
                if (dict.Count > 0)
                {
                    return dict.Dequeue() as T;
                }
                else
                {
                    return ResourcesManager.instance.GetUI<T>();
                    //return GameObject.Instantiate(Resources.Load<T>("UI/" + typeof(T).ToString()));
                }
            }
            return ResourcesManager.instance.GetUI<T>();
            //return GameObject.Instantiate(Resources.Load<T>("UI/" + typeof(T).ToString()));
        }

        void Push<T>(T ui)
            where T : BaseUI
        {
            var t = typeof(T);
            Debug.Log("Push t=" + t);
            if (!_UIPool.ContainsKey(t))
            {
                _UIPool[t] = new Queue<BaseUI>();
            }
            _UIPool[t].Enqueue(ui);
        }

        public T Open<T>(NavigationData data = null, bool isJumpBack = false)
            where T : BaseUI
        {
            if (isJumpBack)
            {
                while (CurFullScreenUI != null && CurFullScreenUI.GetType() != typeof(T))
                {
                    PopupLastFullScreenUI();
                }
                return null;
            }

            Debug.Log("Open t=" + typeof(T));
            var ui = Get<T>();
            if (ui == null)
            {
                Debug.LogError("get ui failed! t=" + typeof(T));
                return null;
            }
            ui.transform.SetParent(_UILayers[ui._NaviData._Layer]);
            ui.CanvasComp.sortingOrder = (int)ui._NaviData._Layer;
            ui.Open(data);
            if (ui._NaviData._Type == EUIType.FullScreen)
            {
                HideFullScreenUI(CurFullScreenUI);
            }
            AddTargetUI(ui);

            return ui;
        }

        public void Close<T>(T ui)
            where T : BaseUI
        {
            CloseInternal(ui);
        }

        // 关闭最后一个该类型的UI
        public void Close<T>()
            where T : BaseUI, new()
        {
            var t = typeof(T);
            T temp = new T();
            BaseUI ui = FindLastUI<T>(temp._NaviData._Type);
            if (ui == null)
            {
                //Debug.LogErrorFormat("do not contains ui with type of {0}", t);
                return;
            }
            CloseInternal(ui as T);
        }

        void CloseInternal<T>(T ui)
            where T : BaseUI
        {
            if (ui == null)
            {
                Debug.LogError("argument is invalid! ui is empty! ");
                return;
            }

            Debug.Log("Close t=" + ui.GetType());

            // 移除栈中各UI
            RemoveTargetUI(ui);

            // 目前只有FullScreen类型的支持导航
            // 处理全屏界面的共存UI
            // 显示新的UI
            if (ui._NaviData._Type == EUIType.FullScreen)
            {
                ShowFullScreenUI(CurFullScreenUI);
            }

            // 推入对象池
            // 这里看看有什么优化方案，目前是Push和ui.CloseInternal分别判断了一次_CloseByDestroy
            if (!ui._NaviData._CloseByDestroy)
            {
                Push(ui);
            }

            ui.Close();
        }

        void ShowFullScreenUI<T>(T ui)
            where T : BaseUI
        {
            if (ui == null)
            {
                return;
            }

            ui.Show();
            if (_CoexistingUI.ContainsKey(ui))
            {
                var list = _CoexistingUI[ui];
                for (int i = 0, length = list.Count; i < length; i++)
                {
                    var l = list[i];
                    l.Show();
                }
            }
        }

        void HideFullScreenUI<T>(T ui)
            where T : BaseUI
        {
            if (ui == null)
            {
                return;
            }

            ui.Hide();
            if (_CoexistingUI.ContainsKey(ui))
            {
                // 仅支持关闭所有共存界面，不可关闭其中的几个
                var list = _CoexistingUI[ui];
                if (ui._NaviData._IsCloseCoexistingUI)
                {
                    for (int i = 0, length = list.Count; i < length; i++)
                    {
                        var l = list[i];
                        CloseByClassName(l.GetType(), l);
                    }
                    list.Clear();
                }
                else
                {
                    for (int i = 0, length = list.Count; i < length; i++)
                    {
                        var l = list[i];
                        l.Hide();
                    }
                }
            }
        }



        void RemoveTargetUI<T>(T ui)
            where T : BaseUI
        {
            if (ui == null)
            {
                return;
            }
            if (ui._NaviData._Type == EUIType.FullScreen)
            {
                if (_OpenedFullScreenUI.Contains(ui))
                {
                    _OpenedFullScreenUI.Remove(ui);
                }
                if (_CoexistingUI.ContainsKey(ui))
                {
                    var list = _CoexistingUI[ui];
                    for (int i = 0, length = list.Count; i < length; i++)
                    {
                        var l = list[i];
                        CloseByClassName(l.GetType(), l);
                    }
                    list.Clear();
                    _CoexistingUI.Remove(ui);
                }
            }
            else if (ui._NaviData._Type == EUIType.Coexisting)
            {
                if (CurFullScreenUI != null && _CoexistingUI.ContainsKey(CurFullScreenUI))
                {
                    _CoexistingUI[CurFullScreenUI].Remove(ui);
                }
            }
            else if (ui._NaviData._Type == EUIType.Independent)
            {
                if (_IndependentUI.Contains(ui))
                {
                    _IndependentUI.Remove(ui);
                }
            }
            else if (ui._NaviData._Type == EUIType.Resident)
            {
                if (_ResidentUI.Contains(ui))
                {
                    _ResidentUI.Remove(ui);
                }
            }
        }

        BaseUI FindLastUI<T>(EUIType type)
        {
            BaseUI ui = null;
            var t = typeof(T);
            if (type == EUIType.FullScreen)
            {
                ui = _OpenedFullScreenUI.FindLast((BaseUI tempUI) => tempUI.GetType() == t);
            }
            else if (type == EUIType.Coexisting)
            {
                if (CurFullScreenUI != null && _CoexistingUI.ContainsKey(CurFullScreenUI))
                {
                    ui = _CoexistingUI[CurFullScreenUI].FindLast((BaseUI tempUI) => tempUI.GetType() == t);
                }
            }
            else if (type == EUIType.Independent)
            {
                ui = _IndependentUI.FindLast((BaseUI tempUI) => tempUI.GetType() == t);
            }
            else if (type == EUIType.Resident)
            {
                ui = _ResidentUI.FindLast((BaseUI tempUI) => tempUI.GetType() == t);
            }
            return ui;
        }

        void AddTargetUI<T>(T ui)
            where T : BaseUI
        {
            if (ui == null)
            {
                return;
            }
            // 设置层级
            var last = FindLastUI<T>(ui._NaviData._Type);
            if (last != null)
            {
                ui.CanvasComp.sortingOrder = last.CanvasComp.sortingOrder + 1;
            }

            if (ui._NaviData._Type == EUIType.FullScreen)
            {
                _OpenedFullScreenUI.Add(ui);
            }
            else if (ui._NaviData._Type == EUIType.Coexisting)
            {
                if (CurFullScreenUI != null)
                {
                    if (!_CoexistingUI.ContainsKey(CurFullScreenUI))
                    {
                        _CoexistingUI[CurFullScreenUI] = new List<BaseUI>();
                    }
                    _CoexistingUI[CurFullScreenUI].Add(ui);
                }
            }
            else if (ui._NaviData._Type == EUIType.Independent)
            {
                _IndependentUI.Add(ui);
            }
            else if (ui._NaviData._Type == EUIType.Resident)
            {
                _ResidentUI.Add(ui);
            }
        }

        public void PopupLastFullScreenUI()
        {
            CloseByClassName(CurFullScreenUI.GetType(), CurFullScreenUI);
        }

        void CloseByClassName(Type t, params object[] args)
        {
            System.Reflection.MethodInfo mi = this.GetType().GetMethod("CloseByReflect").MakeGenericMethod(new Type[] { t });
            mi.Invoke(this, args);
        }

        public void CloseByReflect<T>(T ui)
            where T : BaseUI
        {
            CloseInternal<T>(ui);
        }
    }
}