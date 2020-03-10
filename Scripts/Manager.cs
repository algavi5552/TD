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
    int totalEnemies = 8;
    [SerializeField]
    int enemiesPerSpawn;
    int totalMoney = 30;//стартовые деньги
    int totalEscaped = 0;
    int totalKilled = 0;
    gameStatus currentState = gameStatus.play;

    public List<Enemy> EnemyList = new List<Enemy>();// список врагов на карте

    const float spawnDelay = 0.5f;//появление моба каждые 0,5с

    public int TotalEscaped
    {
        get
        {
            return totalEscaped;//получаем инфо о сбежавших врагах
        }
        set
        {
            totalEscaped = value;
        }
    }
    public int TotalKilled
    {
        get
        {
            return totalKilled;//получаем инфо об убитых врагах
        }
        set
        {
            totalKilled = value;
        }
    }
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
                    GameObject newEnemy = Instantiate(enemies[0]) as GameObject;//спавним первый в списке вид врага
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

    public void IsWaveOver()
    {
        if ((totalEscaped + TotalKilled) == totalEnemies)//если все враги сбежали или убиты
        {
            SetCurrentGamestate();
            ShowMenu();
        }
    }
    public void SetCurrentGamestate()//определим, в каком состоянии нах-ся игра
    {
        if(totalEscaped>=3)//если врагов сбежало больше """3"""
        {
            currentState = gameStatus.gameover;
        }
        else if ((TotalKilled + TotalEscaped) == 0)//если уровень еще не начат
        {
            currentState = gameStatus.play;
        }
        else if (TotalKilled>=totalEnemies-2)//если убито врагов хотя бы 6
        {
            currentState = gameStatus.win;
        }
    }

    public void PlayButtonPressed()
    {
                totalEnemies = 8;
                TotalEscaped = 0;//при начале игры сбежавших 0
                TotalMoney = 30;
                TowerManager.Instance.DestroyAllTowers();//в начале игры уничтожаем башни
                TowerManager.Instance.RenameTagBuildSite();
                totalMoneyLabel.text = TotalMoney.ToString();
               
        DestroyEnemies();//удалим врагов в начале раунда
        TotalKilled = 0;
        StartCoroutine(Spawn());//начинаем растянуто спавнить врагов
        playBtn.gameObject.SetActive(false);//отключаем кнопку play

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
