using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : Loader<Manager>
{
    [SerializeField]
    GameObject spawnPoint;
    [SerializeField]
    GameObject[] enemies;
    [SerializeField]
    int maxEnemiesOnScreen;
    [SerializeField]
    int totalEnemies;
    [SerializeField]
    int enemiesPerSpawn;

    public List<Enemy> EnemyList = new List<Enemy>();// список врагов на карте

    const float spawnDelay = 0.5f;//появление моба каждые 0,5с

    
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        if (enemiesPerSpawn>0 && EnemyList.Count < totalEnemies)
        {
            for (int i=0; i< enemiesPerSpawn; i++)
            {
                if (EnemyList.Count< maxEnemiesOnScreen)
                {
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }
    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);//добавляет врага в лист врагов на экране
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);//удаляет врага из списка врагов на экране
        Destroy(enemy.gameObject);
    }

    public void DestroyEnemies()
    {
        foreach(Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

}
