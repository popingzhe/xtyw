using UnityEngine;

[System.Serializable]

public class ItemDetails 
{
    public int itemID;
    public string name;
    public ItemType itemType;
    //ͼ��
    public Sprite itemIcon;
    public Sprite itemOnWorldSprite;
    public string itemDescription;
    public int itemUseRadius;
    public bool canPickUp;
    public bool canDropped;
    public bool canCarried;
    public int itemPrice;
    [Range(0, 1)]
    public float SellPercentage;
}

