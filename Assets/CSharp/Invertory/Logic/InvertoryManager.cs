using JetBrains.Annotations;
using System;
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


    private void OnEnable()
    {
        EventHander.DropItemEvent += OnDropItemEvent;
        EventHander.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnDisable()
    {
        EventHander.DropItemEvent -= OnDropItemEvent;
        EventHander.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
    }

    private void OnHarvestAtPlayerPosition(int ID)
    {
        var index = GetItemIndexInBag(ID);

        AddItemAtIndex(ID, index, 1);

        //����ui
        EventHander.CallUpdateInvertoryUI(InvertoryLocation.Bag, playerBag.itemList);
    }

    private void Start()
    {
        EventHander.CallUpdateInvertoryUI(InvertoryLocation.Bag, playerBag.itemList);
    }
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

        //����ui
        EventHander.CallUpdateInvertoryUI(InvertoryLocation.Bag, playerBag.itemList); 
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


    private int GetItemIndexInBag(int ID)
    {
        {
            var index = -1;
            for (var i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                {
                    return i;
                }
            }
            return index;
        }
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
                        var newItem = new InvertoryItem() { itemID = ID, itemAmount = amount };
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
            var item = new InvertoryItem() { itemID = ID, itemAmount = newCount };
            playerBag.itemList[index] = item;
        }
    }


    //������Ʒ
    public void SwapItem(int fromIndex, int targetIndex)
    {
        InvertoryItem currentItem = playerBag.itemList[fromIndex];
        InvertoryItem targetItem = playerBag.itemList[targetIndex];

        if(targetItem.itemAmount  != 0)
        {
            playerBag.itemList[fromIndex] = targetItem;
            playerBag.itemList[targetIndex] = currentItem;
        }
        else
        {
            playerBag.itemList[targetIndex] = currentItem;
            playerBag.itemList[fromIndex] = new InvertoryItem();
        }
        EventHander.CallUpdateInvertoryUI(InvertoryLocation.Bag, playerBag.itemList);
    }

    //�Ƴ�ָ�������ı�����Ʒ
    private void RemoveItem(int ID,int removeAmount)
    {
        var index = GetItemIndexInBag(ID);
       
        if (playerBag.itemList[index].itemAmount > removeAmount)
        {
            
            var amount = playerBag.itemList[index].itemAmount - removeAmount;
            var item = new InvertoryItem{itemID = ID,itemAmount = amount };
            playerBag.itemList[index] = item;
        }
        else if(playerBag.itemList[index].itemAmount == removeAmount)
        {
            var item = new InvertoryItem();
            playerBag.itemList[index] = item;
        }

        EventHander.CallUpdateInvertoryUI(InvertoryLocation.Bag,playerBag.itemList);
    }



    private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)
    {
        RemoveItem(ID, 1);
    }

}
