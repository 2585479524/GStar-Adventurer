using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBas<T> : MonoBehaviour where T:SingletonBas<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).ToString());
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }
}
