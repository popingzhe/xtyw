using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorOvrrtide : MonoBehaviour
{
    private Animator[] animators;

    public SpriteRenderer holdItem;

    [Header("各部分动画")]
    public List<Animatortype> animatortypes;
    
    private Dictionary<string,Animator> animatorNameDict = new Dictionary<string,Animator>();

    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        foreach (var item in animators)
        {
            animatorNameDict.Add(item.name, item);
        }
    }

    private void OnEnable()
    {
        EventHander.ItemSelectEvent += OnItemSelectEvent;
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
    }

    private void OnDisable()
    {
        EventHander.ItemSelectEvent -= OnItemSelectEvent;
        EventHander.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHander.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
    }

    private void OnHarvestAtPlayerPosition(int ID)
    {
        Sprite itemSprite = InvertoryManager.Instance.GetItemDetails(ID).itemOnWorldSprite;
        if(holdItem.enabled == false)
        {
            StartCoroutine(ShowItem(itemSprite));
        }

    }

    private IEnumerator ShowItem(Sprite itemSprite)
    {
        holdItem.sprite = itemSprite;
        holdItem.enabled = true;
        yield return new WaitForSeconds(1f);
        holdItem.enabled = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        holdItem.enabled = false;
        SwitchAnimator(PartType.None);
    }

    private void OnItemSelectEvent(ItemDetails details, bool isSelected)
    {
        PartType currentType = details.itemType switch
        {
            ItemType.Seed => PartType.Carry,
            ItemType.Commodity => PartType.Carry,
            ItemType.HoeTool => PartType.Hoe,
            ItemType.WaterTool => PartType.Water,
            ItemType.CollectTool => PartType.Collect,
            ItemType.ChopTool => PartType.Chop,
            _ => PartType.None,
        };

        if(isSelected == false)
        {
            currentType = PartType.None;
            holdItem.enabled = false;
        }
        else
        {
            if(currentType == PartType.Carry)
            {
                holdItem.sprite = details.itemOnWorldSprite;
                holdItem.enabled = true;
            }
            else
            {
                SwitchAnimator(PartType.None);
                holdItem.enabled = false;
            }
        }
       
        SwitchAnimator(currentType);

    }


    private void SwitchAnimator(PartType partType)
    {
       // Debug.Log(partType);
        foreach (var item in animatortypes)
        {
            if(item.partType == partType)
            {
            //    Debug.Log(item.partName);
                animatorNameDict[item.partName.ToString()].runtimeAnimatorController = item.overrideController;
            }
        }
    }
}
