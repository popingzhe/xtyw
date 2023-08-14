using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{


    [Header("种地瓦片切换信息")]
    public RuleTile digTile;
    public RuleTile waterTile;
    private Tilemap digTileMap;
    private Tilemap waterTileMap;
    private Season currentSeason;

    [Header("地图信息")]
    public List<MapData_SO> mapDataList;


    private static Dictionary<string,TileDetails> tileDetailsDict = new Dictionary<string,TileDetails>();

    Grid currentGrid;

    private void OnEnable()
    {
        EventHander.ExecuteActionAfterAnimation += OnExecuteActionAfterAnimation;
        EventHander.AfterSceneloadEvent += OnAfterSceneloadEvent;
        EventHander.GameDayEvenet += OnCallGameDayEvenet;
        EventHander.RefreshCurrentMap += RefreshMap;
    }

    private void OnDisable()
    {
        EventHander.ExecuteActionAfterAnimation -= OnExecuteActionAfterAnimation;
        EventHander.AfterSceneloadEvent -= OnAfterSceneloadEvent;
        EventHander.GameDayEvenet -= OnCallGameDayEvenet;
        EventHander.RefreshCurrentMap -= RefreshMap;
    }


    private void OnAfterSceneloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        digTileMap = GameObject.FindWithTag("Dig").GetComponent<Tilemap>();
        waterTileMap = GameObject.FindWithTag("Water1").GetComponent<Tilemap>();

        //加载数据
        DisPlayMap(SceneManager.GetActiveScene().name);
    }

    private void OnCallGameDayEvenet(int day, Season season)
    {
        currentSeason = season;
        foreach (var item in tileDetailsDict)
        {
            if(item.Value.daySinceWatered > -1)
            {
                item.Value.daySinceWatered = -1; 
            }
            if(item.Value.daySinceDug >  -1)
            {
                item.Value.daySinceDug++;
            }

            if(item.Value.daySinceDug > 5 && item.Value.seedItem == -1)
            {
                item.Value.daySinceDug = -1;
                item.Value.canDig = true;
                item.Value.growthDays = -1;
            }

            if(item.Value.seedItem != -1)
            {
                item.Value.growthDays++;
            }
        }
        RefreshMap();
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

    //设置相应瓦片
    private void SetDigGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.girdX, tile.girdY, 0);
        if(digTileMap != null)
        {
            digTileMap.SetTile(pos, digTile);
        }
    }

    private void SetWaterGround(TileDetails tile)
    {
        Vector3Int pos = new Vector3Int(tile.girdX, tile.girdY, 0);
        if (waterTileMap != null)
        {
            waterTileMap.SetTile(pos, waterTile);
        }
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
                case ItemType.Seed:
                    {
                        EventHander.CallPlantSeedEvent(details.itemID, currentTile);
                        EventHander.CallDropItemEvent(details.itemID, mousePos, details.itemType);
                        break;
                    }
                case ItemType.Commodity:
                    { 
                        EventHander.CallDropItemEvent(details.itemID,mousePos,details.itemType);
                        break;
                    }
                case ItemType.HoeTool:
                    {
                        SetDigGround(currentTile);
                        currentTile.daySinceDug = 0;
                        currentTile.canDig = false;
                        currentTile.canDropItem = false;
                        //音效

                        break;
                    }
                case ItemType.WaterTool:
                    {
                        SetWaterGround(currentTile);
                        currentTile.daySinceWatered = 0;
                        //音效
                        break;
                    }
                case ItemType.ChopTool:
                case ItemType.CollectTool:
                    {
                        Crop currentCrop = GetCropObject(mousePos);
                        if (currentCrop != null)
                        {
                             
                            currentCrop.ProcessToolAction(details,currentTile);
                        }
                        break;
                    }
            }
            UpdateTileDetails(currentTile);
        }

    }

    private Crop GetCropObject(Vector3 mousePos)
    {
        Collider2D[] collider2s = Physics2D.OverlapPointAll(mousePos);

        Crop currentCrop = null;

        for (int i = 0; i < collider2s.Length; i++)
        {
            if (collider2s[i].GetComponent<Crop>())
            {
                currentCrop = collider2s[i].GetComponent<Crop>();
            }
        }
        return currentCrop;
    }

    //更新数据
    private void UpdateTileDetails(TileDetails tileDetails)
    {
        string key = tileDetails.girdX + "x" + tileDetails.girdY + "y" + SceneManager.GetActiveScene().name;
        if (tileDetailsDict.ContainsKey(key))
        {
            tileDetailsDict[key] = tileDetails;
        }
    }

    //刷新地图
    private void RefreshMap()
    {
        if(digTileMap != null)
        {
            digTileMap.ClearAllTiles();
        }
        if (waterTileMap != null)
        {
            waterTileMap.ClearAllTiles();
        }

        foreach (var crop in FindObjectsOfType<Crop>())
        {
             Destroy(crop.gameObject);
        }


        DisPlayMap(SceneManager.GetActiveScene().name);
    }

    //初始化数据
    private void DisPlayMap(string sceneName)
    {
        foreach (var tile in tileDetailsDict)
        {
            var key = tile.Key;
            var tileDetails = tile.Value;
            if (key.Contains(sceneName))
            {
                if(tileDetails.daySinceDug > -1)
                {
                    SetDigGround(tileDetails);
                }
                if(tileDetails.daySinceWatered > -1)
                {
                    SetWaterGround(tileDetails); 
                }
                //种子
                if(tileDetails.seedItem > -1)
                {
                    EventHander.CallPlantSeedEvent(tileDetails.seedItem, tileDetails);
                }
            }
        }
    }
}
