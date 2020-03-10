using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    public TowerBtn towerBtnPressed { get; set; }

    SpriteRenderer spriteRenderer;
    private List<TowerControl> TowerList = new List<TowerControl>();//создаем список башен
    private List<Collider2D> BuildList = new List<Collider2D>();
    private Collider2D buildTile;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D>();//при старте считываем коллайдеры башен
        spriteRenderer.enabled = false;//на старте игры отключить картинки башен
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))//если ЛКМ нажата
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);//считываем коорд-ты курсора
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);//луч от 0 в ЛКМ, переменная для placeTower

            if (hit.collider.tag=="TowerSide") //ставим башни только в точки с тегом
            {
                buildTile = hit.collider;
                buildTile.tag = "TowerSideFull";
                RegisterBuildSite(buildTile);
                PlaceTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void RegisterBuildSite(Collider2D buildTag)//делаем список из башен на экране
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower(TowerControl tower)//регистрируем башню в списке башен на экране
    {
        TowerList.Add(tower);
    }

    public void RenameTagBuildSite()//переименовывает тег "занятоБашней" на "свободноДляСтройки"
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "TowerSide";
        }
        BuildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach(TowerControl tower in TowerList)
        {
            Destroy(tower.gameObject);
        }
        TowerList.Clear();
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed!=null)
            //если мы не навели кнопка башни нажата и ЛКМ уже не на ее иконке
        {
            TowerControl newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;//положение новой башни должно совпадать с ЛКМ
            BuyTower(towerBtnPressed.TowerPrice);
            RegisterTower(newTower);
            DisableDrug();//деактивируем картинку башни на курсоре
        }
        
    }

    public void BuyTower(int price) 
    {
        Manager.Instance.subtractMoney(price);//вычитаем деньги
    }

    public void SelectedTower(TowerBtn towerSelected)
    {
        if (towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
            // башня выбирается только если у нас хватает денег
        {
            towerBtnPressed = towerSelected;
            EnableDrug(towerBtnPressed.DragSprite);
        }
    }

    public void FollowMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);//привязали позицию иконки к курсору
        transform.position = new Vector2(transform.position.x, transform.position.y);//так иконка меняет положение
    }
    public void EnableDrug(Sprite sprite)//вкл картинку башни у курсора
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }
    public void DisableDrug()//выкл картинку башни у курсора
    {
        spriteRenderer.enabled = false;
    }
}
