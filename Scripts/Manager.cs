using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    play,gameover,win
}

public class Manager : Loader<Manager>
{
    [SerializeField]
    int totalWaves=1;//максимальное кол-во волн врагов

    [SerializeField]
    Text totalMoneyLabel;//текущее кол-во денег
    [SerializeField]
    Text playBtnLabel;

    [SerializeField]
    Button playBtn;
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
    int totalMoney = 30;//стартовые деньги
    int totalEscaped = 0;
    int roundEscaped = 0;
    int totalKilled = 0;
    gameStatus currentState = gameStatus.play;

    public List<Enemy> EnemyList = new List<Enemy>();// список врагов на карте

    const float spawnDelay = 0.5f;//появление моба каждые 0,5с

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }
    void Start()
    {
        //StartCoroutine(Spawn());
        playBtn.gameObject.SetActive(false);//кнопка play не появяется в начале игры
        ShowMenu();
    }
    private void Update()
    {
        HandlerEscape();//проверяет,не нажат ли Esc
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

    public void addMoney(int amount)//получаем деньги за башни
    {
        TotalMoney += amount;
    }
    public void subtractMoney(int amount)//тратим деньги на башни
    {
        TotalMoney -= amount;
    }
    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.gameover:
                playBtnLabel.text = "Try again";
                break;
            case gameStatus.play:
                playBtnLabel.text = "PLAY";
                break;
            case gameStatus.win:
                playBtnLabel.text = "WIN";
                break;
        }
        playBtn.gameObject.SetActive(true);
    }
    private void HandlerEscape()//отмена выбора башни
    {
        if (Input.GetKeyDown(KeyCode.Escape))//если нажал Esc, то отмена выбора башни
        {
            TowerManager.Instance.DisableDrug();
            TowerManager.Instance.towerBtnPressed=null;// сброс выбора кнопки башни
        }
    }
}
