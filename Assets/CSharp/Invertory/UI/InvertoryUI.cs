using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvertoryUI : MonoBehaviour
{
    public ItemToolTip itemToolTip;

    [Header("拖拽物品")]
    public Image dragItem; 

    [SerializeField] private SoltUI[] playerSolts;

    [Header("玩家背包UI")]
    [SerializeField] private GameObject bagUI;
    private bool bagOpened;

    private void OnEnable()
    {
        EventHander.UpdateInvertoryUI += OnUpdateInvertoryUI;

    }

    private void OnDisable()
    {
        EventHander.UpdateInvertoryUI -= OnUpdateInvertoryUI;
    }


    private void Start()
    {


        for (int i = 0; i < playerSolts.Length; i++)
        {
            playerSolts[i].index = i;
        }
        bagOpened = bagUI.activeInHierarchy; 
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)) 
        {
            ClickBag();
        }
    }

    public void ClickBag()
    {
        bagOpened = !bagOpened;
        bagUI.SetActive(bagOpened);
    }


    //更新高亮
    public void UpdateToHighLight(int index)
    {
        foreach (var slot in playerSolts)
        {
            if(slot.isSelected && slot.index == index)
            {
                slot.slotHighlight.gameObject.SetActive(true);
            }
            else
            {
                slot.isSelected = false;
                slot.slotHighlight.gameObject.SetActive(false);
            }
        }
    }

    private void OnUpdateInvertoryUI(InvertoryLocation location, List<InvertoryItem> list)
    {
        switch (location)
        {
            case InvertoryLocation.Bag:
                for (int i = 0; i < playerSolts.Length; i++)
                {
                    if (list[i].itemAmount > 0)
                    {
                        var item = InvertoryManager.Instance.GetItemDetails(list[i].itemID);
                        playerSolts[i].UpdateSlot(item, list[i].itemAmount);
                    }
                    else
                    {
                        playerSolts[i].UpdateEmptySolt();
                    }
                }
                break;
        }
    }




}
