using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 单例基类
/// MonoBehaviour脚本继承此类则通过T.Instance拿到单例
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class Singleton<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    private static T m_instance = null;

    public static T Instance
    {
        get { return m_instance; }
    }

    protected virtual void Awake()
    {
        m_instance = this as T;
    }
}