using System;
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
public struct InvertoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]

public class Animatortype
{
    public PartType partType;
    public PartName partName;
    public AnimatorOverrideController overrideController;
}

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos) 
    { 
        x = pos.x; y = pos.y; z = pos.z;
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVeector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }

    [System.Serializable]
    public class SceneItem
    {
        public int itemID;
        public SerializableVector3 position;
    }

}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;

    public bool boolTypeValue;

    public GridType gridType;
}

[System.Serializable]
public class TileDetails
{
    public int girdX, girdY;

    public bool canDig;

    public bool canDropItem;

    public bool canPlaceFutniture;

    public bool isNPCObstacle;

    public int daySinceDug = -1;

    public int daySinceWatered =-1;

    public int seedItem = -1;
    
    public int growthDays = -1;

    public int daySinceLastHarvest = -1;
}
