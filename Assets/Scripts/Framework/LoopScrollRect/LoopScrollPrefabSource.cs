using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [System.Serializable]
    public class LoopScrollPrefabSource 
    {
        public const string _PrefabPath = "UI/"; 
        public string prefabName;
        public int poolSize = 5;

        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if (!inited)
            {
                prefabName = _PrefabPath + prefabName; 
                SG.ResourceManager.Instance.InitPool(prefabName, poolSize);
                inited = true;
            }
            return SG.ResourceManager.Instance.GetObjectFromPool(prefabName);
        }

        public virtual void ReturnObject(Transform go)
        {
            //go.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            SG.ResourceManager.Instance.ReturnObjectToPool(go.gameObject);
        }
    }

    [System.Serializable]
    public class LoopScrollPrefabSource_Reference
    {
        public GameObject _ItemPrefab;
        private bool inited = false;
        public virtual GameObject GetObject()
        {
            if (!inited)
            {
                _ItemPrefab.SetActive(false); 
                inited = true;
            }
            var newGO = GameObject.Instantiate(_ItemPrefab); 
            newGO.SetActive(true); 
            newGO.transform.localScale = Vector3.one;
            return newGO;
        }

        public virtual void ReturnObject(Transform go)
        {
            GameObject.Destroy(go.gameObject);
        }
    }
}
