using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;
using Object = UnityEngine.Object;
using UnityEngine.UIElements;

public class ResourceManager
{
    Dictionary<string, UnityEngine.Object> _resources = new Dictionary<string, UnityEngine.Object>();

    public T Load<T>(string key) where T : Object
    {
        if (_resources.TryGetValue(key, out Object resource))
            return resource as T;

        if (typeof(T) == typeof(Sprite))
        {
            key = key + ".sprite";
            if (_resources.TryGetValue(key, out Object temp))
            {
                return temp as T;
            }
        }

        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>($"{key}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {key}");
            return null;
        }

        // Pooling
        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (Managers.Pool.Push(go))
            return;

        Object.Destroy(go);
    }

    #region ��巹����
    public void LoadAsync<T>(string key, Action<T> callback = null) where T : UnityEngine.Object
    {
        if (_resources.TryGetValue(key, out Object resource))
        {
            callback?.Invoke(resource as T);
            return;
        }

        // ��������Ʈ
        string loadKey = key;
        if (key.Contains(".sprites"))
        {
            var asyncOperation = Addressables.LoadAssetAsync<IList<T>>(loadKey);
            asyncOperation.Completed += (op) =>
            {
                // ĳ�� Ȯ��.
                foreach (var result in op.Result)
                {
                    if (_resources.TryGetValue(key, out Object resource))
                    {
                        break;
                    }
                    _resources.Add($"{result.name}.sprite", result);
                }
                callback?.Invoke(op.Result[op.Result.Count - 1]);
            };
        }
        else
        {
            if (key.Contains(".sprite"))
                loadKey = $"{key}[{key.Replace(".sprite", "")}]";

            var asyncOperation = Addressables.LoadAssetAsync<T>(loadKey);

            asyncOperation.Completed += (op) =>
            {
                // ĳ�� Ȯ��.
                if (_resources.TryGetValue(key, out Object resource))
                {
                    callback?.Invoke(op.Result);
                    return;
                }

                _resources.Add(key, op.Result);
                callback?.Invoke(op.Result);
            };
        }
    }


    public void LoadAllAsync<T>(string label, Action<string, int, int> callback) where T : UnityEngine.Object
    {
        var opHandle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        opHandle.Completed += (op) =>
        {
            int loadCount = 0;
            int totalCount = op.Result.Count;

            foreach (var result in op.Result)
            {
                if (result.PrimaryKey.Contains(".sprite"))
                {
                    LoadAsync<Sprite>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }
                else
                {
                    LoadAsync<T>(result.PrimaryKey, (obj) =>
                    {
                        loadCount++;
                        callback?.Invoke(result.PrimaryKey, loadCount, totalCount);
                    });
                }

            }
        };
    }

    public List<string> gogogogo()
    {
        List<string> result = new List<string>();
        foreach (string key in _resources.Keys)
        {
            result.Add(key);
        }
        return result;
    }
    #endregion
}
