using UnityEngine;
using System.Collections;

public class DragFloor : MonoBehaviour
{
    [SerializeField] Camera _Camera; 
    [SerializeField] float _Factor = 2;
    public float _WorldWidth = 100;
    public float _WorldHeight = 100;

    private Vector2 _offsetOnMouseDown;
    private Vector2 _curPosition;
    private Vector2 _velocity; 
    float _MinGap = 0.01f;
    private Vector2 _screenPoint;

    void Update()
    {
        if (_velocity == Vector2.zero)
        {
            return;
        }
        if (!IsInRange(_Camera.transform.position))
        {
            ResetState();
            return;
        }
        Vector2 pos = _Camera.transform.position;
        pos += _velocity * Time.deltaTime; 
        _velocity -= _velocity.normalized * _Factor * Time.deltaTime;
    }

    bool IsInRange(Vector3 pos)
    {
        return Mathf.Abs(pos.x) < _WorldWidth / 2 && Mathf.Abs(pos.y) < _WorldHeight / 2;
    }

    void ResetState()
    {
        _velocity = Vector3.zero; 
        Vector3 pos = _Camera.transform.position;
        if (pos.x <= -_WorldWidth / 2)
        {
            pos.x = -_WorldWidth / 2 + _MinGap;
        }
        else if (pos.x >= _WorldWidth / 2)
        {
            pos.x = _WorldWidth / 2 - _MinGap;
        }
        if (pos.y <= -_WorldHeight / 2)
        {
            pos.y = -_WorldHeight / 2 + _MinGap;
        }
        else if (pos.y >= _WorldHeight / 2)
        {
            pos.y = _WorldHeight / 2 - _MinGap;
        }
        _Camera.transform.position = pos;
    }

    void OnMouseDown()
    {
        _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        _offsetOnMouseDown = transform.position 
            - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        Debugger.LogGreen(LogUtil.GetCurMethodName() + "_offsetOnMouseDown=" + _offsetOnMouseDown); 
    }

    void OnMouseDrag()
    {
        Vector2 _prevPosition = _curPosition;
        var curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 curWorldPos = Camera.main.ScreenToWorldPoint(curScreenPoint); 
        _curPosition = curWorldPos + _offsetOnMouseDown;
        _velocity = (_curPosition - _prevPosition);
        if (!IsInRange(_curPosition))
        {
            ResetState();
        }
        if (_curPosition.x <= -_WorldWidth / 2)
        {
            _curPosition.x = -_WorldWidth / 2 + _MinGap;
        }
        else if (_curPosition.x >= _WorldWidth / 2)
        {
            _curPosition.x = _WorldWidth / 2 - _MinGap;
        }
        if (_curPosition.y <= -_WorldHeight / 2)
        {
            _curPosition.y = -_WorldHeight / 2 + _MinGap;
        }
        else if (_curPosition.y >= _WorldHeight / 2)
        {
            _curPosition.y = _WorldHeight / 2 - _MinGap;
        }
        SetCameraPos(_curPosition);
    }
    void OnMouseUp()
    {
        Debugger.LogGreen(LogUtil.GetCurMethodName());
        if (IsInRange(_Camera.transform.position))
        {
            Vector3 pos = _Camera.transform.position;
            if (pos.x <= -_WorldWidth / 2 + _MinGap || pos.x >= _WorldWidth / 2 - _MinGap)
            {
                _velocity.x = 0;
            }
            if (pos.y <= -_WorldHeight / 2 + _MinGap || pos.y >= _WorldHeight / 2 - _MinGap)
            {
                _velocity.y = 0;
            }
        }
        _velocity *= 10f; 
    }

    void SetCameraPos(Vector3 pos)
        {
        var p = _Camera.transform.position; 
        p.x = _curPosition.x; 
        p.y = _curPosition.y; 
        _Camera.transform.position = p;
    }
}