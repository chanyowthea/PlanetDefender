using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTest
{
    public class Rock : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("rock target name=" + other.name);
            //GameObject.Destroy(this.gameObject); 
        }
    }
}
