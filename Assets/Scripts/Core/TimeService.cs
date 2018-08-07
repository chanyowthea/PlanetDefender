using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeService
{
    private float m_GameTime;
    public float GameTime
    {
        get
        {
            return m_GameTime;
        }
    }
    private float m_LastGameTime;
    public float LastGameTime
    {
        get
        {
            return m_LastGameTime;
        }
    }

    public float FixedDeltaTime
    {
        get
        {
            return m_FixedDeltaTime;
        }
    }
    private float m_DeltaTime;
    public float DeltaTime
    {
        get
        {
            return m_DeltaTime;
        }
    }
    private uint m_TickCount;
    public uint TickCount
    {
        get
        {
            return m_TickCount;
        }
    }
    private bool m_UsingFixedDeltaTime;
    private float m_FixedDeltaTime;
    public void Reset()
    {
        m_GameTime = 0f;
        m_LastGameTime = 0f;
        m_DeltaTime = 0f;
        m_TickCount = 0;
    }
    public void UseFixedDeltaTime(float fixedDeltaTime)
    {
        m_UsingFixedDeltaTime = true;
        m_FixedDeltaTime = fixedDeltaTime;
    }
    public void UpdateTime()
    {
        if (m_TickCount == uint.MaxValue)
        {
            m_TickCount = 1;
        }
        else
        {
            m_TickCount++;
        }
        if (m_UsingFixedDeltaTime)
        {
            m_DeltaTime += m_FixedDeltaTime;
        }
        else
        {
            m_DeltaTime = Time.deltaTime;
        }
        m_LastGameTime = m_GameTime;
        m_GameTime += Time.deltaTime;
    }
    public void ClearDeltaTime()
    {
        m_DeltaTime = 0;
    }
}
