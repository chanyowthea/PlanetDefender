﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoSingleton<GameAssets>
{
    [Header("Prefabs")]
    public Bullet _bulletPrefab;
    public Enemy _rockPrefab;
    public Gold _goldPrefab;
    public GameObject _ExplosionEffect;
    public Material _RatioRectMaterial;
    public Material _LightningMaterial;

    static GameObject _bulletParent;
    public static GameObject bulletParent
    {
        get
        {
            if (_bulletParent == null)
            {
                _bulletParent = new GameObject("BulletParent");
            }
            return _bulletParent;
        }
    }

    static GameObject _rockParent;
    public static GameObject rockParent
    {
        get
        {
            if (_rockParent == null)
            {
                _rockParent = new GameObject("RockParent");
            }
            return _rockParent;
        }
    }

    static GameObject _goldParent;
    public static GameObject goldParent
    {
        get
        {
            if (_goldParent == null)
            {
                _goldParent = new GameObject("GoldParent");
            }
            return _goldParent;
        }
    }

    static GameObject _TurretParent;
    public static GameObject TurretParent
    {
        get
        {
            if (_TurretParent == null)
            {
                _TurretParent = new GameObject("TurretParent");
            }
            return _TurretParent;
        }
    }
}
