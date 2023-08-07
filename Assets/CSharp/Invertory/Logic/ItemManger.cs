using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManger : MonoBehaviour
{
    public Item itemPrefab;

    private Transform itemParent;

    private void OnEnable()
    {
        EventHander.InstantiateiyemInSence += OnInstantiateiyemInSence;
    }

    private void OnDisable()
    {
        EventHander.InstantiateiyemInSence -= OnInstantiateiyemInSence;
    }
    private void Start()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
    }



    private void OnInstantiateiyemInSence(int ID, Vector3 pos)
    {
        var item = Instantiate(itemPrefab,pos,Quaternion.identity,itemParent);
        item.itemID = ID; 
    }

}
