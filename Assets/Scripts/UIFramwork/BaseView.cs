using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramwork
{
    public class BaseView : MonoBehaviour
    {
        //public int _group;
        //BaseView _parent; 
        //List<BaseView> _children; 
        //public ViewConfig _config;

        Canvas _canvas; 
        public Canvas canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = GetComponent<Canvas>();
                }
                return _canvas; 
            }
        }
        public Action<BaseView> _onOpen;
        public Action<BaseView> _onClose;
        public Action<BaseView> _onShow;
        public Action<BaseView> _onHide;

        public virtual void Open()
        {
            Show();
            if (_onOpen != null)
            {
                _onOpen(this); 
            }
        }

        public virtual void Close()
        {
            Hide(); 
            if (_onClose != null)
            {
                _onClose(this);
            }
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            if (_onShow != null)
            {
                _onShow(this);
            }
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            if (_onHide != null)
            {
                _onHide(this);
            }
        }
    }
}
