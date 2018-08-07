using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Test : MonoBehaviour
{
    private System.Object _Lock = new System.Object();

    void Start()
    {
        Thread t = new Thread(InputThread);
        t.Start();

        Thread t1 = new Thread(InputThread);
        t1.Start();
    }

    void InputThread()
    {
        lock (_Lock)
        {
            while (true)
            {
                Debug.Log("enter lock");
            }
        }
        Debug.Log("other thread");
    }
}
