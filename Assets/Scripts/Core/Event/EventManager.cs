using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene
{
    public class EventManager : MonoSingleton<EventManager>
    {
        public List<RockSource> _rockSrcs = new List<RockSource>();
        [SerializeField] float _randomInterval = 0.2f;
        float _lastRandomTime;
        int _MaxDistance = 30; 

        private void Start()
        {
            GameObject goldParent = new GameObject("GoldGeneratorParent");
            Vector3[] goldPoses = new Vector3[]{ new Vector3(_MaxDistance, 0), new Vector3(-_MaxDistance, 0) , 
                new Vector3(0, _MaxDistance) , new Vector3(0, -_MaxDistance) }; 
            for (int i = 0; i < 4; i++)
            {
                GameObject gold = new GameObject("GoldGenerator");
                gold.transform.SetParent(goldParent.transform);
                gold.AddComponent<GoldSource>(); 
                gold.transform.localPosition = goldPoses[i]; 
            }

            GameObject enemyParent = new GameObject("EnemyGeneratorParent");
            var dis = _MaxDistance * 0.71f; 
            Vector3[] enemyPoses = new Vector3[]{ new Vector3(dis, dis), new Vector3(dis, -dis) ,
                new Vector3(-dis, dis) , new Vector3(-dis, -dis) };
            for (int i = 0; i < 4; i++)
            {
                GameObject enemy = new GameObject("EnemyGenerator");
                enemy.transform.SetParent(enemyParent.transform);
                enemy.AddComponent<RockSource>();
                enemy.transform.localPosition = enemyPoses[i];
            }
        }

        private void Update()
        {
            return; 

            //if (Time.time - _lastRandomTime < _randomInterval * Time.timeScale)
            //{
            //    return;
            //}
            //_lastRandomTime = Time.time;

            //if (RandomUtil.instance.random.Next(0, 100) == 0)
            //{
            //    CreateRock(_rockSrcs[0].transform.position);
            //}
        }

        void CreateRock(Vector3 pos)
        {
            var r = GameObject.Instantiate(GameAssets.instance._rockPrefab);
            //r.SetData(pos, 0.02f, Vector3.Normalize(PlanetController.instance.transform.position - pos)); 
        }
    }
}
