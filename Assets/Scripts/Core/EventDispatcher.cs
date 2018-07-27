using System;
using System.Collections.Generic;
using System.Reflection;

public class EventDispatcher : TSingleton<EventDispatcher>
{
    public MethodInfo GetMethod(Type type, string sName)
    {
        return type.GetMethod(sName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
    }

    public Dictionary<EventID, Dictionary<object, List<MethodInfo>>> m_MethodDict = new Dictionary<EventID, Dictionary<object, List<MethodInfo>>>();

    public void DispatchEvent(EventID eventID, params object[] data)
    {
        Dictionary<object, List<MethodInfo>> handler;
        if (m_MethodDict.TryGetValue(eventID, out handler))
        {
            if (handler == null)
            {
                m_MethodDict.Remove(eventID);
                return; 
            }

            foreach (var item in handler)
            {
                if (item.Value == null || item.Value.Count == 0)
                {
                    continue; 
                }

                for (int i = 0, length = item.Value.Count; i < length; i++)
                {
                    var method = item.Value[i];
                    if (method != null)
                    {
                        method.Invoke(item.Key, data);
                    }
                }
            }
        }
    }

    public void RegisterEvent(EventID eventID, object objectWhereTheMethodLay, string methodName)
    {
        var method = GetMethod(objectWhereTheMethodLay.GetType(), methodName); 
        if (method == null)
        {
            return; 
        }

        if (!m_MethodDict.ContainsKey(eventID))
        {
            m_MethodDict.Add(eventID, new Dictionary<object, List<MethodInfo>>()); 
        }

        // EventID对应表
        var dict = m_MethodDict[eventID];
        if (dict == null)
        {
            m_MethodDict[eventID] = new Dictionary<object, List<MethodInfo>>();
            dict = m_MethodDict[eventID]; 
        }

        // objectWhereTheMethodLay对应表
        // method表
        if (!dict.ContainsKey(objectWhereTheMethodLay))
        {
            dict.Add(objectWhereTheMethodLay, new List<MethodInfo>()); 
        }
        var ms = dict[objectWhereTheMethodLay];
        if (ms == null)
        {
            dict[objectWhereTheMethodLay] = new List<MethodInfo>();
            ms = dict[objectWhereTheMethodLay]; 
        }

        if (!ms.Contains(method))
        {
            ms.Add(method);
        }
    }

    public void UnRegisterEvent(EventID eventID, object objectWhereTheMethodLay, string methodName)
    {
        var method = GetMethod(objectWhereTheMethodLay.GetType(), methodName);
        if (method == null)
        {
            return;
        }

        Dictionary<object, List<MethodInfo>> handler;
        if (m_MethodDict.TryGetValue(eventID, out handler))
        {
            if (handler.ContainsKey(objectWhereTheMethodLay))
            {
                var list = handler[objectWhereTheMethodLay]; 
                if (list != null)
                {
                    // 有可能只是名字相同但不是同一个堆
                    var m = list.Find((MethodInfo info) => info.Name == method.Name);
                    if (m != null)
                    {
                        list.Remove(m);
                    }
                }
            }
        }
    }
}