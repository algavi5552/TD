using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    
    Projectile projectile;
    Enemy targetEnemy = null;

    float attackCounter;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach(Enemy enemy in Manager.Instance.EnemyList)//ищем врагов по списку из менеджера
        {
        if (Vector2.Distance(transform.position, enemy.transform.position) <=attackRadius)
                //если дистанция до врага меньше радиуса обстрела
            {
                enemiesInRange.Add(enemy);//включить в список целей данного врага
            }
        }
        return enemiesInRange;
    }
    private Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;//сброс значения БлижнийВраг
        float smallestDistance=float.PositiveInfinity;//ищет врага на максимальном значении

        foreach(Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) < smallestDistance)
            //если это ближайший противник
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;//то выбрать в цель стрельбы его
            }
        }
        return nearestEnemy;
    }
}
