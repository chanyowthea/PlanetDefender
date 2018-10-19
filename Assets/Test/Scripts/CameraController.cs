using UnityEngine;
using System.Collections;
// 将脚本挂载到摄像机上  
public class CameraController : MonoBehaviour
{
    public Camera _MainCamera{ private set; get;}
    public static CameraController Instance { private set; get; }

    private void Awake()
    {
        Instance = this;
        _MainCamera = GetComponent<Camera>(); 
    }

    //[SerializeField] float _Factor = 10;
    //public float _WorldWidth = 100;
    //public float _WorldHeight = 100;

    //private Vector2 _curPosition;
    //private Vector2 _velocity;
    //float _MinGapX;
    //float _MinGapY;
    //const float _MinGap = 0.001f;

    //public float moveSpeed = 10; // 设置相机移动速度
    //Vector2 _LastPosition;


    //private void Start()
    //{
    //    Vector2 bottomLeft = Camera.main.WorldToScreenPoint(new Vector3(-_WorldWidth / 2f, -_WorldHeight / 2f, 0));
    //    Vector2 topRight = Camera.main.WorldToScreenPoint(new Vector3(_WorldWidth / 2f, _WorldHeight / 2f, 0));
    //    Vector2 v2 = topRight - bottomLeft;
    //    float wRatio = Screen.width / v2.x;
    //    float hRatio = Screen.height / v2.y;
    //    _MinGapX = wRatio * _WorldWidth / 2f;
    //    _MinGapY = hRatio * _WorldHeight / 2f;
    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        _curPosition = transform.position;
    //        _velocity = Vector3.zero;
    //    }
    //    // 当按住鼠标右键的时候
    //    if (Input.GetMouseButton(1))
    //    {
    //        // 获取鼠标的x和y的值，乘以速度和Time.deltaTime是因为这个可以是运动起来更平滑  
    //        float h = -Input.GetAxis("Mouse X") * moveSpeed * Time.deltaTime;
    //        float v = -Input.GetAxis("Mouse Y") * moveSpeed * Time.deltaTime;
    //        // 设置当前摄像机移动，y轴并不改变
    //        // 需要摄像机按照世界坐标移动，而不是按照它自身的坐标移动，所以加上Space.World
    //        _LastPosition = _curPosition;
    //        _curPosition += new Vector2(h, v);
    //        if (!IsInRange(_curPosition))
    //        {
    //            ResetState();
    //        }
    //        if (_curPosition.x <= -_WorldWidth / 2 + _MinGapX)
    //        {
    //            _curPosition.x = -_WorldWidth / 2 + _MinGapX;
    //        }
    //        else if (_curPosition.x >= _WorldWidth / 2 - _MinGapX)
    //        {
    //            _curPosition.x = _WorldWidth / 2 - _MinGapX;
    //        }
    //        if (_curPosition.y <= -_WorldHeight / 2 + _MinGapY)
    //        {
    //            _curPosition.y = -_WorldHeight / 2 + _MinGapY;
    //        }
    //        else if (_curPosition.y >= _WorldHeight / 2 - _MinGapY)
    //        {
    //            _curPosition.y = _WorldHeight / 2 - _MinGapY;
    //        }
    //        Vector3 pos = _curPosition;
    //        pos.z = transform.position.z;
    //        transform.position = pos;
    //    }
    //    if (Input.GetMouseButtonUp(1))
    //    {
    //        _velocity = _curPosition - _LastPosition;
    //        if (IsInRange(transform.position))
    //        {
    //            Vector3 p = transform.position;
    //            if (p.x <= -_WorldWidth / 2 + _MinGapX || p.x >= _WorldWidth / 2 - _MinGapX)
    //            {
    //                _velocity.x = 0;
    //            }
    //            if (p.y <= -_WorldHeight / 2 + _MinGapY || p.y >= _WorldHeight / 2 - _MinGapY)
    //            {
    //                _velocity.y = 0;
    //            }
    //        }
    //        _velocity *= 10f;
    //    }

    //    MiniMapController.Instance.SetPos(transform.position);
    //    if (_velocity == Vector2.zero)
    //    {
    //        return;
    //    }

    //    if (Mathf.Abs(transform.position.x) >= _WorldWidth / 2 - _MinGapX)
    //    {
    //        _velocity.x = 0;
    //    }
    //    if (Mathf.Abs(transform.position.y) >= _WorldHeight / 2 - _MinGapY)
    //    {
    //        _velocity.y = 0;
    //    }

    //    Vector2 velocity = _velocity * Time.deltaTime;

    //    // if the next position out of range then reset it. 
    //    var nextPos = this.transform.position + new Vector3(velocity.x, velocity.y, 0);
    //    if (!IsInRange(nextPos))
    //    {
    //        SetCameraPos(ResetPos(nextPos));
    //    }
    //    else
    //    {
    //        this.transform.Translate(velocity.x, velocity.y, 0, Space.World);
    //    }
    //    float delta = _Factor * Time.deltaTime;
    //    _velocity -= _velocity.normalized * delta;
    //    if (Mathf.Abs(_velocity.x) < delta && Mathf.Abs(_velocity.y) < delta)
    //    {
    //        _velocity = Vector3.zero;
    //    }
    //}

    //bool IsInRange(Vector3 pos)
    //{
    //    return Mathf.Abs(pos.x) <= _WorldWidth / 2 - _MinGapX && Mathf.Abs(pos.y) <= _WorldHeight / 2 - _MinGapY;
    //}

    //void ResetState()
    //{
    //    _velocity = Vector3.zero;
    //    SetCameraPos(ResetPos(transform.position));
    //}

    //Vector2 ResetPos(Vector2 pos)
    //{
    //    if (pos.x <= -_WorldWidth / 2 + _MinGapX)
    //    {
    //        pos.x = -_WorldWidth / 2 + _MinGapX;
    //    }
    //    else if (pos.x >= _WorldWidth / 2 - _MinGapX)
    //    {
    //        pos.x = _WorldWidth / 2 - _MinGapX;
    //    }
    //    if (pos.y <= -_WorldHeight / 2 + _MinGapY)
    //    {
    //        pos.y = -_WorldHeight / 2 + _MinGapY;
    //    }
    //    else if (pos.y >= _WorldHeight / 2 - _MinGapY)
    //    {
    //        pos.y = _WorldHeight / 2 - _MinGapY;
    //    }
    //    return pos;
    //}

    //// do not change z pos. 
    //void SetCameraPos(Vector2 p)
    //{
    //    Vector3 pos = p;
    //    pos.z = transform.position.z;
    //    transform.position = pos;
    //}
}