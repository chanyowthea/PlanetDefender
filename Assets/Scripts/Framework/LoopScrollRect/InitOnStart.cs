using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SG
{
    [RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
    [DisallowMultipleComponent]
    public class InitOnStart : MonoBehaviour
    {
        public int totalCount = -1;
        void Start()
        {
            var ls = GetComponent<LoopScrollRect>();
            var objs = new string[] { "Test001", "Jack", "Rolin", "Nick", "Test00656", "Lily", "Niya", "Bonika" ,
                "Test001", "Jack", "Rolin", "Nick", "Test00656", "Lily", "Niya", "Bonika" };
            ls.SetData<string>(objs);
            //ls.totalCount = totalCount;
            ls.RefillCells();
        }
    }
}