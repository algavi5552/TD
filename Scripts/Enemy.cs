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
    [SerializeField]
    int health;
    [SerializeField]
    int rewardAmount;

    int target = 0;
    Transform enemy;//считываем координаты врага
    Collider2D enemyCollider;
    Animator anim;
    float navigationTime = 0;
    bool isDead = false;

    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        Manager.Instance.RegisterEnemy(this);//вызываем RegisterEnemy в менеджере
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints!=null || isDead == false)//если еще остались непокрытые ВП и враг жив
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
        else if (collision.tag=="Projectile")//если снаряд попал во врага
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);//наносим урон снарядами с тегом projectile
            Destroy(collision.gameObject);//снаряд уничтожается
        }
    }
    public void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0)//если еще не умер
        {
            health -= hitPoints;//вычитаем урон из хп
            anim.Play("Hurt");
        }
        else
        {
            Die();
            anim.SetTrigger("didDie");
        }
        
    }
    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;//когда враг погибает.коллайдер отключается
    }
}
