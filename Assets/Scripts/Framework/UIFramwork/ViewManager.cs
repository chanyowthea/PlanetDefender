using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems; 

// 加载来源

// 重要文件
// ViewManager
// BaseView
// 

// 重要概念
// Group
// Child
// Stack
// HashCode 表示同一类型不同id的View

// 方案一
// 一共三个表. 
//      1 viewPool存储已经用过但没有数据的view, 
//      2 _showedViews存储当前显示的view(无论是Open还是Show所显示的), 
//      3 _hidedViews存储隐藏的view
// 各个表存储的数据不重复,三个表加起来就是全部实例化的view
// _navigationViews存储各个view的导航数据

// -- 存在的问题



// 方案二 全部用栈的形式存储


    
namespace UIFramwork
{
    public class ViewManager : TSingleton<ViewManager>
    {
        static Transform _uiParent; 
        public static Transform uiParent
        {
            get
            {
                if (_uiParent == null)
                {
                    _uiParent = new GameObject("UIParent").transform;
                    UnityEngine.Object.DontDestroyOnLoad(_uiParent);
                    var eventSystem = new GameObject("EventSystem");
                    eventSystem.AddComponent<EventSystem>();
                    eventSystem.AddComponent<StandaloneInputModule>();
                    eventSystem.transform.SetParent(_uiParent); 

                    var ns = Enum.GetNames(typeof(ELayer)); 
                    for (int i = 0, length = ns.Length; i < length; i++)
                    {
                        var n = ns[i];
                        ELayer l = (ELayer)Enum.Parse(typeof(ELayer), n);
                        if (!_layers.ContainsKey(l))
                        {
                            var tf = new GameObject(n).transform;
                            tf.SetParent(_uiParent); 
                            _layers.Add(l, tf);
                        }
                    }
                }
                return _uiParent; 
            }
        }
        static Dictionary<ELayer, Transform> _layers = new Dictionary<ELayer, Transform>(); 

        // 一个Type可能对应多个View的实例
        Dictionary<Type, Queue<BaseView>> _viewPool = new Dictionary<Type, Queue<BaseView>>();
        // 保存导航view,关闭当前的view会打开哪个view
        Dictionary<int, List<int>> _navigationViews = new Dictionary<int, List<int>>();
        // 当前显示的所有view,显示的都在这里,没有显示的都在pool里
        Dictionary<int, BaseView> _showedViews = new Dictionary<int, BaseView>();
        // 当前使用的具有数据的view
        Dictionary<int, BaseView> _hideViews = new Dictionary<int, BaseView>();

        public ViewManager()
        {
            var ui = uiParent; 
        }

        /// <summary>
        /// 打开一定是要传入数据的
        /// </summary>
        public T Open<T>(bool isClearNavigation = false)
            where T : BaseView
        {
            T view = null;
            var t = typeof(T);
            // 如果清除导航数据,那么这些导航数据里面可能有所要跳转的view
            if (isClearNavigation)
            {
                foreach (var item in _hideViews)
                {
                    if (item.Value.GetType() == t)
                    {
                        view = item.Value as T;
                        _hideViews.Remove(item.Value.GetHashCode());
                        break; 
                    }
                }
            }
            // 如果上述view获取失败,那么继续Get
            if (view == null)
            {
                view = Get<T>();
            }

            if (view == null)
            {
                Debug.LogError("view == null");
                return null;
            }

            var conf = GameAssets.instance._viewLibrary.GetConfig(typeof(T));
            if (conf == null)
            {
                Debug.LogError("config is empty! ");
                return null;
            }

            var code = view.GetHashCode();
            // 清除这个view之后的导航数据
            if (isClearNavigation)
            {
                ClearNavi(code); 
            }

            // 如果要隐藏其他的view
            if (conf._openType == EOpenType.HideOther)
            {
                HideOther(code, isClearNavigation); 
            }

            _showedViews.Add(code, view);
            SetLayer(view); 

            string s1 = "";
            for (int i = 0, length = _showedViews.Count; i < length; i++)
            {
                s1 += _showedViews.ElementAt(i).Key + ", ";
            }
            Debug.Log(typeof(T) + " after Open _showedViews=" + s1);

            view.Open();
            return view;
        }

        void HideOther(int destViewHashCode, bool isClearNavi)
        {
            // 如果清空导航就直接跳转,不加入导航信息
            if (!isClearNavi)
            {
                if (_navigationViews.ContainsKey(destViewHashCode))
                {
                    _navigationViews.Remove(destViewHashCode);
                }
                _navigationViews.Add(destViewHashCode, new List<int>(_showedViews.Keys));

                //string s = "";
                //for (int i = 0, length = _showedViews.Count; i < length; i++)
                //{
                //    s += _showedViews.ElementAt(i).Key + ", ";
                //}
                //Debug.Log(typeof(T) + " Open _showedViews=" + s);

                //s = "";
                //for (int i = 0, length = _navigationViews.Count; i < length; i++)
                //{
                //    s += _navigationViews.ElementAt(i).Key + ", ";
                //}
                //Debug.Log("code=" + code + ", " + typeof(T) + " Open _navigationViews=" + s);

                for (int i = 0, length = _showedViews.Count; i < length; i++)
                {
                    var v = _showedViews.ElementAt(i).Value;
                    if (v == null)
                    {
                        Debug.LogError("_showedViews element is empty! i=" + i);
                        continue;
                    }
                    v.Hide();
                    if (!_hideViews.ContainsKey(v.GetHashCode()))
                    {
                        _hideViews.Add(v.GetHashCode(), v);
                    }
                }
                _showedViews.Clear();
            }
            else
            {
                for (int i = 0, length = _showedViews.Count; i < length; i++)
                {
                    var v = _showedViews.ElementAt(i).Value;
                    if (v == null)
                    {
                        Debug.LogError("_showedViews element is empty! i=" + i);
                        continue;
                    }
                    var conf1 = GameAssets.instance._viewLibrary.GetConfig(v.GetType());
                    if (conf1._closeType == ECloseType.Hide)
                    {
                        v.Close();
                        Push(v);
                    }
                    else if (conf1._closeType == ECloseType.Destroy)
                    {
                        v.Close();
                        GameObject.Destroy(v.gameObject);
                    }
                }
                _showedViews.Clear();
            }
        }

        void ClearNavi(int destViewHashCode)
        {
            List<int> targetCodes = new List<int>();
            targetCodes.Add(destViewHashCode);
            if (targetCodes.Count > 0)
            {
				// 获取每个显示view的navi表
                // 获取初始表
                var list = new List<int>();
                foreach (var item in _showedViews)
                {
					if(_navigationViews.ContainsKey(item.Key))
					{
						list.AddRange(_navigationViews[item.Key]);
					}
                }

				// 把hashCode之后所有的view全部销毁或者推进对象池
                // 获取初始hashCode
                List<int> tempList = new List<int>();
                while (true)
                {
                    tempList.Clear();
                    tempList.AddRange(list);
                    list.Clear();
                    for (int i = 0, length = tempList.Count; i < length; i++)
                    {
                        var c = tempList[i];
                        BaseView v = null;
                        if (_hideViews.ContainsKey(c))
                        {
                            v = _hideViews[c];
                        }
                        if (v != null)
                        {
                            _hideViews.Remove(c);
                            var conf1 = GameAssets.instance._viewLibrary.GetConfig(v.GetType());
                            if (conf1._closeType == ECloseType.Hide)
                            {
                                v.Close();
                                Push(v);
                            }
                            else if (conf1._closeType == ECloseType.Destroy)
                            {
                                v.Close();
                                GameObject.Destroy(v.gameObject);
                            }
                        }
                        if (!_navigationViews.ContainsKey(c) || _navigationViews[c].Count == 0)
                        {
                            continue;
                        }
                        // 从导航表中获取导航view的hashCode
                        list.AddRange(_navigationViews[c]);
                        _navigationViews.Remove(c);
                    }
                    if (targetCodes.Contains(destViewHashCode) || _navigationViews.Count == 0 || list.Count == 0)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 通过hashCode关闭指定view
        /// 关闭一定要清除数据
        /// </summary>
        public void Close(int hashCode)
        {
            // 删除导航数据
            if (_navigationViews.ContainsKey(hashCode))
            {
                // 打开导航中的view
                var nav = _navigationViews[hashCode];
                for (int i = 0, length = nav.Count; i < length; i++)
                {
                    var c = nav[i];
                    if (!_hideViews.ContainsKey(c))
                    {
                        Debug.LogError("!_inUseViews.ContainsKey(c)");
                        continue;
                    }
                    var v = _hideViews[c];
                    //var v = GetViewByHashCode(nav[i]);
                    if (v == null)
                    {
                        Debug.LogError("v == null");
                        continue;
                    }
					v.Open();
                    _hideViews.Remove(c);
                    _showedViews.Add(c, v);
                }
                _navigationViews.Remove(hashCode);
            }

            // 删除显示view
            if (_showedViews.ContainsKey(hashCode))
            {
                var view = _showedViews[hashCode];
                var conf = GameAssets.instance._viewLibrary.GetConfig(view.GetType());
                if (conf._closeType == ECloseType.Hide)
                {
                    view.Close();
                    Push(view);
                }
                else if (conf._closeType == ECloseType.Destroy)
                {
                    view.Close();
                    GameObject.Destroy(view.gameObject);
                }
                _showedViews.Remove(hashCode);
            }


			string s1 = "";
			for (int i = 0, length = _showedViews.Count; i < length; i++)
			{
				s1 += _showedViews.ElementAt(i).Key + ", ";
			}
			Debug.Log(hashCode + " after Open _showedViews=" + s1);
        }
        
        /// <summary>
        /// 从对象池中获取,获取到了那么从对象池中取出一个,如果没有获取到,那么实例化一个,但不加入对象池
        /// </summary>
        T Get<T>()
            where T : BaseView
        {
            var t = typeof(T);
            if (_viewPool.ContainsKey(t))
            {
                var queue = _viewPool[t];
                if (queue == null)
                {
                    Debug.LogError("list is empty! type=" + t);
                    return null;
                }
                if (queue.Count == 0)
                {
                    return Create<T>();
                }
                var v = _viewPool[t].Dequeue() as T;
                if (v == null)
                {
                    Debug.LogErrorFormat("convert prefab to {0} failed! ", typeof(T));
                    return null;
                }
                return v;
            }
            return Create<T>();
        }

        /// <summary>
        /// 将view清除数据并加入对象池
        /// </summary>
        void Push<T>(T view)
            where T : BaseView
        {
            if (view == null)
            {
                Debug.LogError("view is empty! ");
                return;
            }

            var t = view.GetType();
            if (_viewPool.ContainsKey(t))
            {
                var q = _viewPool[t];
                if (q != null && !q.Contains(view))
                {
                    q.Enqueue(view);
                }
            }
            else
            {
                Queue<BaseView> queue = new Queue<BaseView>();
                queue.Enqueue(view);
                _viewPool.Add(t, queue);
            }
        }

        /// <summary>
        /// 从资源中获取配置并实例化view
        /// </summary>
        T Create<T>()
            where T : BaseView
        {
            var conf = GameAssets.instance._viewLibrary.GetConfig(typeof(T));
            if (conf == null)
            {
                Debug.LogError("conf is empty! ");
                return null;
            }
            var view = GameObject.Instantiate(conf._prefab) as T;
            if (view == null)
            {
                Debug.LogErrorFormat("convert prefab to {0} failed! ", typeof(T));
                return null;
            }
            view.name += "_" + view.GetHashCode();
            view.transform.SetParent(_layers[conf._layer]);
            return view;
        }

        // 只有打开的时候会设置层级
        void SetLayer(BaseView view)
        {
            var conf = GameAssets.instance._viewLibrary.GetConfig(view.GetType());
            if (conf == null)
            {
                view.canvas.sortingOrder = 0;
                return; 
            }

            int count = 0;
            foreach (var item in _showedViews)
            {
                var v = item.Value;
                var con = GameAssets.instance._viewLibrary.GetConfig(v.GetType());
                if (con == null)
                {
                    continue;
                }
                if (con._layer == conf._layer)
                {
                    ++count;
                }
            }
            foreach (var item in _hideViews)
            {
                var v = item.Value;
                var con = GameAssets.instance._viewLibrary.GetConfig(v.GetType());
                if (con == null)
                {
                    continue;
                }
                if (con._layer == conf._layer)
                {
                    ++count;
                }
            }
            view.canvas.sortingOrder = (int)conf._layer + count;
        }

        public T GetShowedView<T>()
            where T : BaseView
        {
            var t = typeof(T); 
            foreach (var item in _showedViews)
            {
                if(item.Value.GetType() == t)
                {
                    return item.Value as T;
                }
            }
            return null; 
        }
    }
}
