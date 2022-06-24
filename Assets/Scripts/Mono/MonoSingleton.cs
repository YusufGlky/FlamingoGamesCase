using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T>: MonoBehaviour where T:MonoSingleton<T>
{
    private static volatile T singleton = null;
    public static T Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType(typeof(T)) as T;
            }
            return singleton;
        }
    }
}
