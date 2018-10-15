using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class UVChainLightning : MonoBehaviour
{
    public float _Displace = 1f;
    public float detail = 1;
    public float _LengthFactor = 2;

    public Vector3 targetPos;
    public Vector3 startPos;
    public float yOffset = 0;
    private LineRenderer _lineRender;
    private List<Vector3> _linePosList;
    float _ChangeIntervalTime = 0.1f;
    float _CurTime;
    Vector3 _TargetDir; 

    private void Awake()
    {
        _lineRender = GetComponent<LineRenderer>();
        _linePosList = new List<Vector3>();
    }

    private void Update()
    {
        if (targetPos == Vector3.zero)
        {
            if (_linePosList.Count != 0)
            {
                //GameManager.instance.DelayCall(1f, () =>
                //{
                    _lineRender.positionCount = 0; 
                    _linePosList.Clear();
                //});
            }
            return;
        }

        //if (Time.timeScale != 0)
        if (GameManager.instance.TimeScale != 0)
        {
            if (_CurTime < _ChangeIntervalTime)
            {
                //_CurTime += Time.deltaTime; 
                _CurTime += GameManager.instance._DelayCallUtil.Timer.DeltaTime;
                return; 
            }
            else
            {
                _CurTime = 0;
            }

            _linePosList.Clear();
            if (targetPos != null)
            {
                targetPos = targetPos + Vector3.up * yOffset;
            }
            if (this.startPos != null)
            {
                startPos = this.startPos + Vector3.up * yOffset;
            }
            _TargetDir = targetPos - startPos; 
            CollectLinPos(startPos, targetPos, Vector3.Distance(startPos, targetPos) * _LengthFactor);
            _linePosList.Add(targetPos);

            _lineRender.positionCount = _linePosList.Count;
            for (int i = 0, n = _linePosList.Count; i < n; i++)
            {
                _lineRender.SetPosition(i, _linePosList[i]);
            }
        }
    }

    //收集顶点，中点分形法插值抖动
    private void CollectLinPos(Vector3 startPos, Vector3 destPos, float displace)
    {
        if (displace < detail)
        {
            _linePosList.Add(startPos);
        }
        else
        {
            float midX = (startPos.x + destPos.x) / 2;
            float midY = (startPos.y + destPos.y) / 2;
            float midZ = (startPos.z + destPos.z) / 2;

            var normal = _TargetDir.normalized;
            var dir = new Vector3(normal.y, -normal.x); 
            midX += (float)(UnityEngine.Random.value - 0.5) * _Displace * dir.x;
            midY += (float)(UnityEngine.Random.value - 0.5) * _Displace * dir.y;
            //midZ += (float)(UnityEngine.Random.value - 0.5) * _Displace;

            Vector3 midPos = new Vector3(midX, midY, midZ);

            // 如果满足条件，那么会加入两个点
            CollectLinPos(startPos, midPos, displace / 2);
            CollectLinPos(midPos, destPos, displace / 2);
        }
    }
}
