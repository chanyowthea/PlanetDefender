using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameTest
{
    public class Bullet : MonoBehaviour
    {
        public int _hp = 1;
        public int _attack;
        public int _defense;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("bullet target name=" + other.name);
            GameObject.Destroy(this.gameObject);
        }
    }
}
