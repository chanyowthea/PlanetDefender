using System.Collections;
using System.Collections.Generic;
using UIFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraInputUI : BaseUI, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] float _Factor = 10;
    public float _WorldWidth = 100;
    public float _WorldHeight = 100;

    // set camera move speed. 
    public float moveSpeed = 100;

    private Vector2 _curPosition;
    private Vector2 _velocity;
    float _MinGapX;
    float _MinGapY;

    Vector2 _LastPosition;

    public CameraInputUI()
    {
        _NaviData._Type = EUIType.Coexisting;
        _NaviData._Layer = EUILayer.FullScreen;
        _NaviData._IsCloseCoexistingUI = false;
    }

    private void Start()
    {
        Vector2 bottomLeft = CameraController.Instance._MainCamera.WorldToScreenPoint(new Vector3(-_WorldWidth / 2f, -_WorldHeight / 2f, 0));
        Vector2 topRight = CameraController.Instance._MainCamera.WorldToScreenPoint(new Vector3(_WorldWidth / 2f, _WorldHeight / 2f, 0));
        Vector2 v2 = topRight - bottomLeft;
        float wRatio = Screen.width / v2.x;
        float hRatio = Screen.height / v2.y;
        _MinGapX = wRatio * _WorldWidth / 2f;
        _MinGapY = hRatio * _WorldHeight / 2f;
    }

    void Update()
    {
        if (MiniMapController.Instance != null && MiniMapController.Instance.gameObject.activeSelf)
        {
            MiniMapController.Instance.SetPos(CameraController.Instance._MainCamera.transform.position);
        }
        if (_velocity == Vector2.zero)
        {
            return;
        }

        if (Mathf.Abs(CameraController.Instance._MainCamera.transform.position.x) >= _WorldWidth / 2 - _MinGapX)
        {
            _velocity.x = 0;
        }
        if (Mathf.Abs(CameraController.Instance._MainCamera.transform.position.y) >= _WorldHeight / 2 - _MinGapY)
        {
            _velocity.y = 0;
        }

        Vector2 velocity = _velocity * Time.deltaTime;

        // if the next position out of range then reset it. 
        var nextPos = CameraController.Instance._MainCamera.transform.position + new Vector3(velocity.x, velocity.y, 0);
        if (!IsInRange(nextPos))
        {
            SetCameraPos(ResetPos(nextPos));
        }
        else
        {
            CameraController.Instance._MainCamera.transform.Translate(velocity.x, velocity.y, 0, Space.World);
        }
        float delta = _Factor * Time.deltaTime;
        _velocity -= _velocity.normalized * delta;
        if (Mathf.Abs(_velocity.x) < delta && Mathf.Abs(_velocity.y) < delta)
        {
            _velocity = Vector3.zero;
        }
    }

    bool IsInRange(Vector3 pos)
    {
        return Mathf.Abs(pos.x) <= _WorldWidth / 2 - _MinGapX && Mathf.Abs(pos.y) <= _WorldHeight / 2 - _MinGapY;
    }

    void ResetState()
    {
        _velocity = Vector3.zero;
        SetCameraPos(ResetPos(CameraController.Instance._MainCamera.transform.position));
    }

    Vector2 ResetPos(Vector2 pos)
    {
        if (pos.x <= -_WorldWidth / 2 + _MinGapX)
        {
            pos.x = -_WorldWidth / 2 + _MinGapX;
        }
        else if (pos.x >= _WorldWidth / 2 - _MinGapX)
        {
            pos.x = _WorldWidth / 2 - _MinGapX;
        }
        if (pos.y <= -_WorldHeight / 2 + _MinGapY)
        {
            pos.y = -_WorldHeight / 2 + _MinGapY;
        }
        else if (pos.y >= _WorldHeight / 2 - _MinGapY)
        {
            pos.y = _WorldHeight / 2 - _MinGapY;
        }
        return pos;
    }

    // do not change z pos. 
    void SetCameraPos(Vector2 p)
    {
        Vector3 pos = p;
        pos.z = CameraController.Instance._MainCamera.transform.position.z;
        CameraController.Instance._MainCamera.transform.position = pos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _curPosition = CameraController.Instance._MainCamera.transform.position;
        _LastPosition = _curPosition;
        _velocity = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 获取鼠标的x和y的值，乘以速度和Time.deltaTime是因为这个可以是运动起来更平滑  
        float h = -Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
        float v = -Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
        // 设置当前摄像机移动，y轴并不改变
        // 需要摄像机按照世界坐标移动，而不是按照它自身的坐标移动，所以加上Space.World
        _LastPosition = _curPosition;
        _curPosition += new Vector2(h, v);
        if (!IsInRange(_curPosition))
        {
            ResetState();
        }
        if (_curPosition.x <= -_WorldWidth / 2 + _MinGapX)
        {
            _curPosition.x = -_WorldWidth / 2 + _MinGapX;
        }
        else if (_curPosition.x >= _WorldWidth / 2 - _MinGapX)
        {
            _curPosition.x = _WorldWidth / 2 - _MinGapX;
        }
        if (_curPosition.y <= -_WorldHeight / 2 + _MinGapY)
        {
            _curPosition.y = -_WorldHeight / 2 + _MinGapY;
        }
        else if (_curPosition.y >= _WorldHeight / 2 - _MinGapY)
        {
            _curPosition.y = _WorldHeight / 2 - _MinGapY;
        }
        Vector3 pos = _curPosition;
        pos.z = CameraController.Instance._MainCamera.transform.position.z;
        CameraController.Instance._MainCamera.transform.position = pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _velocity = _curPosition - _LastPosition;
        if (IsInRange(CameraController.Instance._MainCamera.transform.position))
        {
            Vector3 p = CameraController.Instance._MainCamera.transform.position;
            if (p.x <= -_WorldWidth / 2 + _MinGapX || p.x >= _WorldWidth / 2 - _MinGapX)
            {
                _velocity.x = 0;
            }
            if (p.y <= -_WorldHeight / 2 + _MinGapY || p.y >= _WorldHeight / 2 - _MinGapY)
            {
                _velocity.y = 0;
            }
        }
        _velocity *= 10f;
    }
}
