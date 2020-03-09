using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
//public class TowerManager : MonoBehaviour
{
    TowerBtn towerBtnPressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        Debug.Log("Pressed"+ towerBtnPressed.gameObject);
    }
}
