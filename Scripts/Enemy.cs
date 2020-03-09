using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    

    [SerializeField]
    Transform exit;
    [SerializeField]
    Transform[] wayPoints;

    [SerializeField]
    float navigation;//частота обновления перса

    int target = 0;
    Transform enemy;//считываем координаты врага
    float navigationTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        Manager.Instance.RegisterEnemy(this);//вызываем RegisterEnemy в менеджере
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints!=null)//если еще остались непокрытые ВП
        {
            navigationTime += Time.deltaTime;
            if (navigationTime < navigation)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                    //двигаться к след. ВП
                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                    //если ВП кончились, то на выход
                }
                navigationTime = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Waypoint")
        {
            target++;
        }
        else if(collision.tag == "Finish")
        {
            Manager.Instance.UnregisterEnemy(this);
        }
    }
}
