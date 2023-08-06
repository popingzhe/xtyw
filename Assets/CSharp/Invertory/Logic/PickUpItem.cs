using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();

        if (item != null)
        {
            
            if (item.itemDetails.canPickUp)
            {
                InvertoryManager.Instance.AddItem(item,true); 
            }
        }
    }
}
