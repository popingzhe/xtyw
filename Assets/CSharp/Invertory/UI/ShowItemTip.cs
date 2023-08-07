using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SoltUI))]
public class ShowItemTip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private SoltUI soltUI;
    private InvertoryUI invertoryUI;


    private void Awake()
    {
        soltUI = GetComponent<SoltUI>();
        invertoryUI = GetComponentInParent<InvertoryUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (soltUI.itemAmount > 0)
        {
            invertoryUI.itemToolTip.gameObject.SetActive(true);
            invertoryUI.itemToolTip.SetToolTip(soltUI.itemDetails, soltUI.type);

            invertoryUI.itemToolTip.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
            invertoryUI. itemToolTip.transform.position =transform.position + Vector3.up*60;
        }
        else
        {
            invertoryUI.itemToolTip.gameObject.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        invertoryUI.itemToolTip.gameObject.SetActive(false);
    }
}
