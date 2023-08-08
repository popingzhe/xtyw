using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SerializableVector3;

public class ItemManger : MonoBehaviour
{
    public Item itemPrefab;

    private Transform itemParent;

    //管理场景物品
    private Dictionary<string, List<SceneItem>> sceneItems = new Dictionary<string, List<SceneItem>>();

    private void OnEnable()
    {
        EventHander.InstantiateiyemInSence += OnInstantiateiyemInSence;
        EventHander.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent += OnAfterSceneUnloadEvent;

    }


    private void OnDisable()
    {
        EventHander.InstantiateiyemInSence -= OnInstantiateiyemInSence;
        EventHander.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHander.AfterSceneloadEvent -= OnAfterSceneUnloadEvent;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        GetAllSceneItem();
    }

    private void OnAfterSceneUnloadEvent()
    {
        itemParent = GameObject.FindWithTag("ItemParent").transform;
        ReCreateAllItem();
    }


    private void OnInstantiateiyemInSence(int ID, Vector3 pos)
    {
        var item = Instantiate(itemPrefab,pos,Quaternion.identity,itemParent);
        item.itemID = ID; 
    }
    private void GetAllSceneItem()
    {
        List<SceneItem> currentSceneItems = new List<SceneItem>();
        foreach (var item in FindObjectsOfType<Item>())
        {

            SceneItem nItem = new SceneItem();
            nItem.itemID = item.itemID;
            nItem.position = new SerializableVector3(item.transform.position);
            Debug.Log("添加物品：" +nItem.itemID);
            currentSceneItems.Add(nItem);
            //清除
            Destroy(item.gameObject);
        }
        if (sceneItems.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneItems[SceneManager.GetActiveScene().name]=currentSceneItems;
        }
        else
        {
            sceneItems.Add(SceneManager.GetActiveScene().name, currentSceneItems);
        }
    }

    //重建物品
    private void ReCreateAllItem()
    {
        List<SceneItem> currentItems = new List<SceneItem>();
        if(sceneItems.TryGetValue(SceneManager.GetActiveScene().name,out currentItems))
        {
            if(currentItems != null)
            {
                foreach (var item in currentItems)
                {
                    Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                    newItem.Init(item.itemID);
                }
            }
        }
    }
}
