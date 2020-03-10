using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField]
    float timeBetweenAttacks;
    [SerializeField]
    float attackRadius;
    [SerializeField]
    Projectile projectile;
    Enemy targetEnemy = null;
    bool isAttacking = false;

    float attackCounter;//задержка между выстрелами  для нашей башни

    void Update()
    {
        attackCounter -= Time.deltaTime;//запускаем счет между выстрелами

        if (targetEnemy == null || targetEnemy.IsDead)//если нет врага в прицеле или враг умер
        {
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Vector2.Distance
                (transform.localPosition, nearestEnemy.transform.localPosition) <= attackRadius)//если рядом есть враг
            {
                targetEnemy = nearestEnemy;//взять врага на прицел
            }
        }
        else
        {
            if (attackCounter<=0f)
            {
                isAttacking = true;//стреляем по откату

                attackCounter = timeBetweenAttacks;//сброс счетчика стрельбы
            }
            else
            {
                isAttacking = false;
            }
            if (Vector2.Distance(transform.localPosition, targetEnemy.transform.localPosition) > attackRadius)
            //если враг вышел за пределы стрельбы
            {
                targetEnemy = null;//снимаем врага с прицела
            }
            
        }
        
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while(GetTargetDistance(targetEnemy)>0.20f && projectile!=null && targetEnemy!=null)
            //условия стрельбы
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;//расстояние должно уменьшаться
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //снаряд должен доворачивать
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards
                (projectile.transform.localPosition, targetEnemy.transform.localPosition, 4f*Time.deltaTime);
            //снаряд движется к цели
            yield return null;
        }
        if (projectile!=null|| targetEnemy==null)//если снаряд есть,а цели нет
        {
            Destroy(projectile);
        }
    }

    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();//если нет врага, то взять на прицел ближайшего
            if (thisEnemy == null)
            {
                return 0f;
            }
        }
        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
        //постоянно считываем расстояние до противника
    }
    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
            Attack();
        }
    }
    public void Attack()
    {
        isAttacking = false;//башня изначально не стреляет
        Projectile newProjectile = Instantiate(projectile) as Projectile;//клонируем снаряд
        newProjectile.transform.localPosition=transform.localPosition;//снаряд появляется в башне
        if (targetEnemy == null)
        {
            Destroy(newProjectile);//если врагов в прицеле нет, удаляем снаряд
        }
        else
        {
            //двигаем снаряд к врагу
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach(Enemy enemy in Manager.Instance.EnemyList)//ищем врагов по списку из менеджера
        {
        if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <=attackRadius)
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
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            //если это ближайший противник
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;//то выбрать в цель стрельбы его
            }
        }
        return nearestEnemy;
    }
}
