using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoltUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    [Header("组件获取")]
    [SerializeField] private Image slotImage;
    [SerializeField] private TextMeshProUGUI amoutText;
    [SerializeField] public Image slotHighlight;
    [SerializeField] private Button button;
    [Header("格子类型")] 
    public SlotType type;
    public bool isSelected;
    //索引
    public int index;

    //物品信息
    public ItemDetails itemDetails;
    public int itemAmount;

    private InvertoryUI invertoryUI =>GetComponentInParent<InvertoryUI>();

    private void Start()
    {
        isSelected = false;
        if(itemDetails.itemID == 0)
        {
            UpdateEmptySolt(); 
        }
    }

    //更新ui
    public void UpdateSlot(ItemDetails item,int amount)
    {
        itemDetails = item;
        slotImage.sprite = item.itemIcon;
        slotImage.enabled = true;
        itemAmount = amount;
        amoutText.text = amount.ToString();
        button.interactable = true;
    }

    //更新为空格子
    public void UpdateEmptySolt()
    {
        if (isSelected)
        {
            isSelected = false;
        }
        slotImage.enabled = false;
        amoutText.text = string.Empty;
        button.interactable = false;
        itemAmount = 0;

    }


    //点击回调
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemAmount <= 0) return;
        isSelected = !isSelected;
        slotHighlight.gameObject.SetActive(isSelected);

        invertoryUI.UpdateToHighLight(index);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemAmount <= 0) return;       
            invertoryUI.dragItem.enabled = true;
            invertoryUI.dragItem.sprite = slotImage.sprite;
            invertoryUI.dragItem.SetNativeSize();

            isSelected = true;
            invertoryUI.UpdateToHighLight(index);
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        invertoryUI.dragItem.enabled = false;
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject);
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SoltUI>() == null)
                return;

            var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SoltUI>();
            int targetIndex = targetSlot.index;

            if(type == SlotType.Bag && targetSlot.type == SlotType.Bag)
            {
                InvertoryManager.Instance.SwapItem(index,targetIndex);
            }

            //清空高亮显示
            invertoryUI.UpdateToHighLight(-1);
        }
        else
        {
            if (itemDetails.canDropped)
            {
                var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
                EventHander.CallInstantiateiyemInSence(itemDetails.itemID, pos);
            }
 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        invertoryUI.dragItem.transform.position = Input.mousePosition;

    }
}
