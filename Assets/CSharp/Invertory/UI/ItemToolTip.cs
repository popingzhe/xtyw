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
            ItemType.Seed => "����",
            ItemType.Commodity => "��Ʒ",
            ItemType.CollectTool => "����",
            ItemType.ReapTool => "����",
            ItemType.HoeTool => "����",
            ItemType.BreakTool => "����",
            ItemType.ChopTool => "����",
            _ => "����"
        };
    }
}
