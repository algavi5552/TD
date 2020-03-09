using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    TowerBtn towerBtnPressed;

    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
                hit.collider.tag = "TowerSideFull";
                PlaceTower(hit);
            }
        }
        if (spriteRenderer.enabled)
        {
            FollowMouse();
        }
    }

    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed!=null)
            //если мы не навели кнопка башни нажата и ЛКМ уже не на ее иконке
        {
            GameObject newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;//положение новой башни должно совпадать с ЛКМ
            DisableDrug();//деактивируем картинку башни на курсоре
        }
        
    }
    public void SelectedTower(TowerBtn towerSelected)
    {
        towerBtnPressed = towerSelected;
        EnableDrug(towerBtnPressed.DragSprite);
        Debug.Log("Pressed"+ towerBtnPressed.gameObject);
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
