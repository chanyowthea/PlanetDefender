using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //缩放系数
    public float scaleFactor = 10f;

    //记录上一次手机触摸位置判断用户是在左放大还是缩小手势
    private Vector2 oldPosition1;
    private Vector2 oldPosition2;
    [SerializeField] Camera m_Camera;
    float distance;
    
    //这个变量用来记录单指双指的变换
    private bool m_IsSingleFinger;

    void Update()
    {
        //判断触摸数量为单点触摸
        if (Input.touchCount == 1)
        {
            m_IsSingleFinger = true;
        }
        if (Input.touchCount > 1)
        {
            //当从单指触摸进入多指触摸的时候,记录一下触摸的位置
            //保证计算缩放都是从两指手指触碰开始的
            if (m_IsSingleFinger)
            {
                oldPosition1 = Input.GetTouch(0).position;
                oldPosition2 = Input.GetTouch(1).position;
                m_IsSingleFinger = false;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                //计算出当前两点触摸点的位置
                var tempPosition1 = Input.GetTouch(0).position;
                var tempPosition2 = Input.GetTouch(1).position;

                float currentTouchDistance = Vector3.Distance(tempPosition1, tempPosition2);
                float lastTouchDistance = Vector3.Distance(oldPosition1, oldPosition2);

                //计算上次和这次双指触摸之间的距离差距
                distance = (currentTouchDistance - lastTouchDistance) * scaleFactor * Time.deltaTime;
                distance *= -1; 
                m_Camera.fieldOfView = Mathf.Clamp(m_Camera.fieldOfView + distance, GameConfig.instance._MinFOV, GameConfig.instance._MaxFOV);

                //备份上一次触摸点的位置，用于对比
                oldPosition1 = tempPosition1;
                oldPosition2 = tempPosition2;
            }
        }
    }
}
