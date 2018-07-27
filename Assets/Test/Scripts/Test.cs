using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTest
{
    public class Test : MonoBehaviour
    {
        [SerializeField] LineRenderer _line;
        [SerializeField] int _attack = 1;
        [SerializeField] int _defense;
        void Start()
        {
            //atk * (1 - def / (def + 100))

            var angle = Vector3.Angle(new Vector3(1, 0, 0), new Vector3(0, 1, 0));
            Debug.Log(angle); 
        }

        void Update()
        {
            //_line.positionCount = 100; 
            //for (int i = 0, length = 100; i < length; i++)
            //{
            //    var r = _attack * (1 - i / (float)(_attack + i)); 
            //    _line.SetPosition(i, new Vector3(i / 10f, r * 10, 0)); 
            //}


        }
    }
}