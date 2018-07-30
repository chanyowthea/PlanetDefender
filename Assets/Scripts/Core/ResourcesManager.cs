using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : TSingleton<ResourcesManager>
{
    public Dictionary<string, UnityEngine.Object> _LoadedAssetDict = new Dictionary<string, Object>();
    public const string _PicturePrefix = "Textures/";

    UnityEngine.Object Load(string path)
    {
        return Resources.Load(path);
    }

    public void UnloadAll()
    {
        Resources.UnloadUnusedAssets();
    }

    public void UnloadAsset(UnityEngine.Object asset)
    {
        Resources.UnloadAsset(asset);
    }

    public T GetAsset<T>(string path)
        where T : UnityEngine.Object
    {
        if (_LoadedAssetDict.ContainsKey(path))
        {
            return (T)_LoadedAssetDict[path];
        }
        return (T)Load(path);
    }

    public Sprite GetSprite(string path)
    {
        return GetAsset<Sprite>(_PicturePrefix.Append(path));
    }
}
