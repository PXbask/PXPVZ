﻿using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public bool global = true;
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance =(T)FindObjectOfType<T>();
            }
            return instance;
        }

    }

    void Awake()
    {
        if (global) {
            if (instance != null && GetComponent<T>() != instance)
            {
                Destroy(gameObject);
                return;
            }
            instance = GetComponent<T>();
            Transform obj = transform;
            while(obj.parent != null) { obj = obj.parent; }
            DontDestroyOnLoad(obj.gameObject);
        } 
        this.OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}