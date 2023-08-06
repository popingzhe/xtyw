using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InvertoryManager : Singleton<InvertoryManager>
{
    [Header("��Ʒ����")]
    public ItemDataList_So itemDataList_So;

    [Header("��������")]
    public InertoryBag_SO playerBag;

    //������Ʒ
    public ItemDetails GetItemDetails(int ID)
    {
        return itemDataList_So.itemDataList.Find(i=>i.itemID == ID);
    }


    //�����Ʒ������
    public void AddItem(Item item,bool toDestory)
    {
        var index = GetItemIndex(item);

        AddItemAtIndex(item.itemID, index, 1);
        
        
        if(toDestory)
        {
            Destroy(item.gameObject);
        }
    }

    private bool CheckBagCapicity()
    {
        for (var i = 0; i < playerBag.itemList.Count; i++)
        {
            if (playerBag.itemList[i].itemID == 0)
                return true;
        }
        return false;
    }

    private int GetItemIndex(Item itemI) 
    {
        var index = -1;
        for (var i = 0;i<playerBag.itemList.Count;i++)
        {
            if (playerBag.itemList[i].itemID == itemI.itemID)
            {
                return i;
            }
        }
        return index;
    }
    private void AddItemAtIndex(int ID,int index,int amount)
    {
        if (index == -1 )//û����Ʒ
        {
            if (CheckBagCapicity())
            {
                for (var i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        var newItem = new InertoryItem() { itemID = ID, itemAmount = amount };
                        playerBag.itemList[i] = newItem;
                        break;
                    }

                }
            }
            else
            {
                Debug.Log("̫���ˣ�װ���£���");
            }
        }
        else
        {
            var newCount = playerBag.itemList[index].itemAmount + amount;
            var item = new InertoryItem() { itemID = ID, itemAmount = newCount };
            playerBag.itemList[index] = item;
        }
    }
}
