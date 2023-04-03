using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    public static T Instance;
    
    protected void Awake()
    {
        if(Instance == null)
        {
            Instance = this as T;
        }
        else
        {
            Destroy(this);
            Debug.Log($"Singleton {this as T} duplicado");
        }
    }
}
