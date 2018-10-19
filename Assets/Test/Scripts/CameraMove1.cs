using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove1 : MonoBehaviour
{
    [SerializeField] float _Factor = 10; 
    //定义摄像机可以活动的范围
    public float xMin = -100;
    public float xMax = 100;
    public float yMin = -100;
    public float yMax = 100;

    private Vector2 lastSingleTouchPosition;
    bool m_IsSingleFinger;
    private Vector3 m_CameraOffset;
    private Camera m_Camera;


    //初始化游戏信息设置
    void Start()
    {
        m_Camera = this.GetComponent<Camera>();
        m_CameraOffset = m_Camera.transform.position;
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        if (Input.GetMouseButtonDown(0))
        {
            lastSingleTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            MoveCamera(Input.mousePosition);
        }
#else
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began || !m_IsSingleFinger)
            {
                //在开始触摸或者从两字手指放开回来的时候记录一下触摸的位置
                lastSingleTouchPosition = Input.GetTouch(0).position;
            }
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                MoveCamera(Input.GetTouch(0).position);
            }
            m_IsSingleFinger = true;
        }
#endif
    }

    private void MoveCamera(Vector3 scenePos)
    {
        Vector3 lastTouchPostion = m_Camera.ScreenToWorldPoint(new Vector3(lastSingleTouchPosition.x, lastSingleTouchPosition.y, -1));
        Vector3 currentTouchPosition = m_Camera.ScreenToWorldPoint(new Vector3(scenePos.x, scenePos.y, -1));

        Vector3 v = currentTouchPosition - lastTouchPostion;
        m_CameraOffset += new Vector3(v.x, v.y, 0) * _Factor;

        //把摄像机的位置控制在范围内
        m_CameraOffset = new Vector3(Mathf.Clamp(m_CameraOffset.x, xMin, xMax), Mathf.Clamp(m_CameraOffset.y, yMin, yMax), m_CameraOffset.z);
        lastSingleTouchPosition = scenePos;

        var position = m_CameraOffset + m_Camera.transform.forward;
        m_Camera.transform.position = m_CameraOffset;
    }
}
