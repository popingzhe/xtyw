using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTip : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private GameObject bottomPart;

    public void SetToolTip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        descriptionText.text = itemDetails.itemDescription;
        typeText.text = GetItemType(itemDetails.itemType);

        if(itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity|| itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);

            var price = itemDetails.itemPrice;
            if(slotType == SlotType.Bag)
            {
                price = (int) (price *itemDetails.SellPercentage);
            }

            priceText.text = price.ToString();
        }
        else
        {
            bottomPart.SetActive(false);
        }
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.CollectTool => "工具",
            ItemType.ReapTool => "工具",
            ItemType.HoeTool => "工具",
            ItemType.BreakTool => "工具",
            ItemType.ChopTool => "工具",
            _ => "其他"
        };
    }
}
