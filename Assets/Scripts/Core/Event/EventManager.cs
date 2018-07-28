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

        private void Update()
        {
            if (Time.time - _lastRandomTime < _randomInterval * Time.timeScale)
            {
                return;
            }
            _lastRandomTime = Time.time;

            if (RandomUtil.instance.random.Next(0, 100) == 0)
            {
                CreateRock(_rockSrcs[0].transform.position);
            }
        }

        void CreateRock(Vector3 pos)
        {
            var r = GameObject.Instantiate(GameAssets.instance._rockPrefab);
            r.SetData(pos, 0.02f, Vector3.Normalize(PlanetController.instance.transform.position - pos)); 
        }
    }
}
