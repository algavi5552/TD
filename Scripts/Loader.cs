﻿using UnityEngine;

public class Loader <T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;//менеджера изначально нет на сцене

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            else if (instance != FindObjectOfType<T>())
            {
                Destroy(FindObjectOfType<T>());
            }
            DontDestroyOnLoad(FindObjectOfType<T>());

            return instance;
        }
    }
}