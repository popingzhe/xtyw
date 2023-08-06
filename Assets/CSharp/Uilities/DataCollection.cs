using UnityEngine;

[System.Serializable]

public class ItemDetails 
{
    public int itemID;
    public string itemName;
    public ItemType itemType;
    //Í¼±ê
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

[System.Serializable]
public struct InertoryItem
{
    public int itemID;
    public int itemAmount;
}
