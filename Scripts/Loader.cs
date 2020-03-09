using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject manager;

    void Awake()
    {
        if(Manager.instance==null)
        {
            Instantiate(manager);//если манагера нет, создать его
        }
    }

}
