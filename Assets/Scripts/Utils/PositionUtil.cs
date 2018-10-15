using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionUtil : MonoBehaviour
{
    /// <summary>
    /// 在指定位置,朝指定方向以一定速度移动一段时间后,物体所在的位置
    /// </summary>
    /// <param name="sourcePos"></param>
    /// <param name="moveDir">单位向量</param>
    /// <param name="moveSpeed">米/帧</param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Vector3 GetTargetPos(Vector3 sourcePos, Vector3 moveDir, float moveSpeed, float time)
    {
        return sourcePos + moveDir * moveSpeed * time;
    }

    /// <summary>
    /// 获取两直线交点
    /// </summary>
    public static Vector3 GetTargetPos(Vector3 sourcePos, Vector3 moveDir, Vector3 firePos, Vector3 fireDir)
    {
        return GetIntersection(sourcePos, sourcePos + moveDir, firePos, firePos + fireDir);
    }

    /// <summary>
    /// 两点式求两直线交点
    /// </summary>
    public static Vector3 GetIntersection(Vector3 p00, Vector3 p01, Vector3 p10, Vector3 p11)
    {
        float k1 = 0;
        float k2 = 0;
        int state = 0;
        Debugger.DrawLine(p00, p01, Color.yellow);
        Debugger.DrawLine(p10, p11, Color.yellow);
        // 第一个点是否是非竖线
        if (p00.x != p01.x)
        {
            // 计算斜率
            k1 = (p01.y - p00.y) / (p01.x - p00.x);
            state |= 1 << 0;
        }

        // 第二个点是否是非竖线
        if (p10.x != p11.x)
        {
            k2 = (p11.y - p10.y) / (p11.x - p10.x);
            state |= 1 << 1;
        }
        float x = 0;
        float y = 0; 
        switch (state)
        {
            //L1与L2都平行Y轴
            case 0:
                return Vector3.zero; 
            case 1: //L1存在斜率, L2平行Y轴
                x = p10.x;
                y = (x - p00.x) * k1 + p00.y;
                return new Vector3(x, y);
            case 2: //L1平行Y轴，L2存在斜率
                {
                    x = p00.x;
                    y = (x - p10.x) * k2 + p10.y;
                    return new Vector3(x, y);
                }
            case 3: //L1，L2都存在斜率
                {
                    if (k1 == k2)
                    {
                        return Vector3.zero; 
                    }
                    float x1 = (k1 * p00.x - k2 * p10.x - p00.y + p10.y) / (k1 - k2);
                    float y1 = k1 * x1 - k1 * p00.x + p00.y;
                    return new Vector3(x1, y1);
                }
        }
        return Vector3.zero; 
    }
    //L1，L2都存在斜率的情况：
    //直线方程L1: ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 )
    //=> y = [ ( y2 - y1 ) / ( x2 - x1 ) ]( x - x1 ) + y1
    //令 a = ( y2 - y1 ) / ( x2 - x1 )
    //有 y = a * x - a * x1 + y1   .........1
    //直线方程L2: ( y - y3 ) / ( y4 - y3 ) = ( x - x3 ) / ( x4 - x3 )
    //令 b = ( y4 - y3 ) / ( x4 - x3 )
    //有 y = b * x - b * x3 + y3 ..........2

    //如果 a = b，则两直线平等，否则， 联解方程 1,2，得:
    //x = ( a * x1 - b * x3 - y1 + y3 ) / ( a - b )
    //y = a * x - a * x1 + y1

    //L1存在斜率, L2平行Y轴的情况：
    //x = x3
    //y = a * x3 - a * x1 + y1

    //L1 平行Y轴，L2存在斜率的情况：
    //x = x1
    //y = b * x - b * x3 + y3

    //L1与L2都平行Y轴的情况：
    //如果 x1 = x3，那么L1与L2重合，否则平等

    public static float GetTimeToTargetPos(Vector3 sourcePos, Vector3 targetPos, float moveSpeed)
    {
        return Vector3.Magnitude(targetPos - sourcePos) / moveSpeed;
    }
}