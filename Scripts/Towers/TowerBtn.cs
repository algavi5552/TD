using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    GameObject towerObject;
    [SerializeField]
    Sprite dragSprite;
    [SerializeField]
    int towerPrice;

    public GameObject TowerObject
    {
        get
        {
            return towerObject;
        }
    }

    public Sprite DragSprite
    {
        get
        {
            return dragSprite;//возвращает картинку выбранной кнопки
        }
    }
    public int TowerPrice
    {
        get
        {
            return towerPrice;
        }
    }
}
