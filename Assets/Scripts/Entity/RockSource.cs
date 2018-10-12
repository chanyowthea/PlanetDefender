using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scene;

public class RockSource : MonoBehaviour
{
    [SerializeField] float _generateInterval = 2;
    float _lastTime;

    private void Start()
    {
        EventManager.instance._rockSrcs.Add(this);
    }

    void Update()
    {
        if (GameManager.instance._DelayCallUtil.GameTime - _lastTime < _generateInterval)
        {
            return;
        }
        _lastTime = GameManager.instance._DelayCallUtil.GameTime;
        var r = GameObject.Instantiate(GameAssets.instance._rockPrefab);
        r.transform.position = this.transform.position;
        var dir = Vector3.Normalize(PlanetController.instance.transform.position -
            new Vector3(1 - RandomUtil.instance.random.Next(10000) / 10000f * 2, 1 - RandomUtil.instance.random.Next(10000) / 10000f * 2, 0));
        //r.SetData(transform.position, 0.02f, dir, EFaction.Enemy, new System.Random().Next(1, 23));
        r.SetData(transform.position, 0.02f, dir, EFaction.Enemy, 3);
    }
}
