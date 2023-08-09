using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Singleton<MapManager>
{
    [Header("地图信息")]
    public List<MapData_SO> mapDataList;

    private static Dictionary<string,TileDetails> tileDetailsDict = new Dictionary<string,TileDetails>();

    Grid currentGrid;

    private void OnEnable()
    {
        EventHander.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
        EventHander.AfterSceneloadEvent += OnAfterSceneloadEvent;
    }

    private void OnDisable()
    {
        EventHander.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
        EventHander.AfterSceneloadEvent -= OnAfterSceneloadEvent;
    }

    private void OnAfterSceneloadEvent()
    {
      currentGrid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        foreach (var mapData in mapDataList)
        {
            InitTileDetailsDict(mapData);
        }
    }

    private void InitTileDetailsDict(MapData_SO mapData)
    {
        foreach (TileProperty tileProperty in mapData.tileProperties)
        {
            TileDetails tileDetails = new TileDetails
            {
                girdX = tileProperty.tileCoordinate.x,
                girdY = tileProperty.tileCoordinate.y,
            };

            //key
            string key = tileDetails.girdX + "x" + tileDetails.girdY + "y" + mapData.sceneName;
            if (GetTileDetails(key)!=null)
            {
                tileDetails = GetTileDetails(key);
            }

            switch(tileProperty.gridType)
            {
                case GridType.Diggable:
                    tileDetails.canDig = tileProperty.boolTypeValue; break;
                case GridType.NPCObstacle:
                    tileDetails.isNPCObstacle = tileProperty.boolTypeValue; break;
                case GridType.DropItem:
                    tileDetails.canDropItem = tileProperty.boolTypeValue; break;
                case GridType.PlaceFurniture:
                    tileDetails.canPlaceFutniture = tileProperty.boolTypeValue; break;
            }

            if (GetTileDetails(key) != null)
            {
                tileDetailsDict[key] = tileDetails;
            }
            else
            {
               
                tileDetailsDict.Add(key, tileDetails);
            }
             
        }
    }

    private static TileDetails GetTileDetails(string key)
    {
        if (tileDetailsDict.ContainsKey(key))
        {
            return tileDetailsDict[key];
        }
        else
        {
            return null;
        }
            
    }

    //网格坐标返回信息
    public static TileDetails GetTileDetailsOnMousePosition(Vector3Int mouseGridPos)
    {
        string key = mouseGridPos.x + "x" + mouseGridPos.y + "y" + SceneManager.GetActiveScene().name;
        return GetTileDetails(key);
    }


    //事件可以执行，生成相应结果
    private void OnExecuteActionAfterAnimation(Vector3 mousePos, ItemDetails details)
    {
        var mouseGridPos = currentGrid.WorldToCell(mousePos);
        var currentTile = GetTileDetailsOnMousePosition(mouseGridPos);

        if (currentTile != null)
        {
            switch (details.itemType)
            {
                case ItemType.Commodity:
                    {
 
                        EventHander.CallDropItemEvent(details.itemID,mousePos);
                        break;
                    }
            }
        }

    }
}
