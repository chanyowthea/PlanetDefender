using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoopScrollRectItem<T>
{
    void SetData(T data);
    void ClearData(); 
}
