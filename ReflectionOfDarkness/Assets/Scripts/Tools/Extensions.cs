using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Mirror;
using Debug = UnityEngine.Debug;
using System.Reflection;

public static partial class Extensions
{
    public static GameObject FindWithTag(this string tagName)
    {
        if (string.IsNullOrEmpty(tagName) || string.IsNullOrWhiteSpace(tagName))
            return default;

        return GameObject.FindWithTag(tagName);
    }
    public static string LogMethod(this object obj)
    {
        return LogMethod();
    }
    public static string LogMethod()
    {
        MethodBase method = Method(3);
        return string.Format("{0} called by {1}", method.Name, method.ReflectedType.Name);
    }
    public static string LogMethodByNetworkIdentity(this NetworkBehaviour networkBehaviour)
    {
        MethodBase method = Method(3);
        return string.Format("{0} called by {1} (u:{2})", method.Name, method.ReflectedType.Name, networkBehaviour.netId);
    }
    /// <summary>
    /// Kullanýldýðý yerin Methodu döner
    /// </summary>
    /// <param name="frameIndex">kaç tane geri method gideceðini belirtir.</param>
    /// <returns>MethodBase</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static MethodBase Method(int frameIndex = 1)
    {
        var st = new StackTrace();

        var sf = st.GetFrame(frameIndex);
        return sf.GetMethod();
    }
    /// <summary>
    /// Kullanýldýðý yerin Method adýný geriye döner
    /// </summary>
    /// <param name="frameIndex">kaç tane geri method gideceðini belirtir.</param>
    /// <returns>Methodun adý</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string MethodName(int frameIndex = 2)
    {
        return Method(frameIndex).Name;
    }
    public static Entity Entity<T>(this T mono) where T : MonoBehaviour
    {
        return mono.GetComponent<Entity>();
    }
    public static Source Is<Source, Destination>(this Source source, System.Action<Destination> action)
    {
        if (action != null && source is Destination destination && destination != null)
        {
            action.Invoke(destination);
            return default;
        }

        return source;
    }

    public static T HasComponent<T>(this GameObject gameObject)
    {
        if (gameObject is null)
            return default;

        return gameObject.GetComponent<T>();
    }
    public static bool HasComponent<T>(this GameObject gameObject, out T t)
    {
        t = default;

        if (gameObject is null)
            return false;

        t = gameObject.GetComponent<T>();

        return t != null;
    }

    public static bool HasTag(this GameObject gameObject, EntityType gameObjectTags)
    {
        return HasTagAll(gameObject, gameObjectTags);
    }

    public static bool HasTagAny(this GameObject gameObject, params EntityType[] gameObjectTags)
    {
        return !HasTagControl(gameObjectTags) && HasTagCore(gameObject, gameObjectTags) > 0;
    }

    public static bool HasTagAll(this GameObject gameObject, params EntityType[] gameObjectTags)
    {
        return !HasTagControl(gameObjectTags) && HasTagCore(gameObject, gameObjectTags) == gameObjectTags.Length;
    }

    private static bool HasTagControl(EntityType[] gameObjectTags)
    {
        return gameObjectTags is null || gameObjectTags.Length == 0;
    }

    private static int HasTagCore(this GameObject gameObject, params EntityType[] gameObjectTags)
    {
        int count = 0;

        if (gameObject is null)
        {
            return count;
        }

        Entity tagManagament = gameObject.GetComponent<Entity>();

        if (tagManagament == null)
        {
            return count;
        }

        if (gameObjectTags == null || gameObjectTags.Length == 0)
        {
            return count;
        }

        for (int a = 0; a < gameObjectTags.Length; a++)
        {
            bool has = tagManagament.entities.Contains(gameObjectTags[a]);

            if (has)
            {
                ++count;
            }
        }

        return count;
    }

    public static float Limit(this float currentValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        if (minValue != float.MinValue && currentValue < minValue)
        {
            return minValue;
        }
        else if (maxValue != float.MaxValue && currentValue > maxValue)
        {
            return maxValue;
        }
        else
        {
            return currentValue;
        }
    }

    public static int Limit(this int currentValue, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        if (minValue != int.MinValue && currentValue < minValue)
        {
            return minValue;
        }
        else if (maxValue != int.MaxValue && currentValue > maxValue)
        {
            return maxValue;
        }
        else
        {
            return currentValue;
        }
    }
}