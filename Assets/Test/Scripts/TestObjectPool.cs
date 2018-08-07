using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameTest
{
    class AudioResource : IObjectPoolCallback
    {
        public int _Number; 
        
        public void OnAllocated()
        {
            _Number = 0; 
        }

        public void OnCollected()
        {
            _Number = -1; 
        }
    }

    public class TestObjectPool : MonoBehaviour
    {
        ObjectPool<AudioResource> _AudioPool = new ObjectPool<AudioResource>(); 
        
        private void Start()
        {
            List<AudioResource> list = new List<AudioResource>(); 
            for (int i = 0; i < 3; i++)
            {
                var a = _AudioPool.AllocObject();
                Debug.Log("1 alloc " + a._Number); 
                a._Number = (i + 1) * 5;
                Debug.Log("1 list " + a._Number);
                list.Add(a); 
            }

            for (int i = 0, length = list.Count; i < length; i++)
            {
                _AudioPool.CollectObject(list[i]); 
            }
            list.Clear();

            for (int i = 0; i < 4; i++)
            {
                var a = _AudioPool.AllocObject();
                Debug.Log("2 alloc " + a._Number);
                a._Number = (i + 1) * 6;
                Debug.Log("2 list " + a._Number);
                list.Add(a);
            }
        }
    }
}