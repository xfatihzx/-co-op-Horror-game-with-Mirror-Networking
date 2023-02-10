using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable // for name rules
public static class Singleton
{
    public static Dictionary<Type, object> instances = new Dictionary<Type, object>();
    public static T GetAndSet<T>(this CacheBehaviour<T> cacheBehaviour) where T : UnityEngine.Object
    {
        lock (instances)
        {
            var type = typeof(T);

            if (!instances.Keys.Contains(type))
            {
                instances.Add(type, cacheBehaviour);

                GameObject.DontDestroyOnLoad(cacheBehaviour.gameObject);
            }

            return (T)instances[type];
        }
    }
    public static bool TryGet<T>(out T obj) where T : UnityEngine.Object
    {
        lock (instances)
        {
            var type = typeof(T);

            bool HasValue = instances.Keys.Contains(type);

            obj = HasValue ? (T)instances[type] : null;

            return HasValue;
        }
    }
}
public class CacheBehaviour<T> : MonoBehaviour where T : UnityEngine.Object
{
    [NonSerialized]
    public T instance;

    public static T singleton
    {
        get
        {
            Singleton.TryGet<T>(out T instance);

            return instance;
        }
    }

    private void Awake()
    {
        instance = Singleton.GetAndSet(this);
    }
}
#pragma warning restore