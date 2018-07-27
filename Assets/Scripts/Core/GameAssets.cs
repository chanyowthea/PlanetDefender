using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoSingleton<GameAssets>
{
    [Header("ItemPrefabs")]
    public GameObject _cannonPrefab;
    public Bullet _bulletPrefab;
    public Rock _rockPrefab;
    public Gold _goldPrefab;

    [Header("Materials")]
    public Material _rockMat;
    public Material _rockMat_Default;

    [Header("Configs")]
    public ViewLibrary _viewLibrary;


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
}
