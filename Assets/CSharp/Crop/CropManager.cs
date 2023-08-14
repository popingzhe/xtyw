using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CropManager : Singleton<CropManager>
{
    public CropDataList_SO cropData;

    public Transform cropParent;

    private Grid currentGrid;

    private Season currentSeason;
    private void OnEnable()
    {
        EventHander.PlantSeedEvent += OnPlantSeedEvent;
        EventHander.AfterSceneloadEvent += OnAfterSceneloadEvent;
        EventHander.GameDayEvenet += OnGameDayEvenet;
    }


    private void OnDisable()
    {
        EventHander.PlantSeedEvent -= OnPlantSeedEvent;
        EventHander.AfterSceneloadEvent -= OnAfterSceneloadEvent;
        EventHander.GameDayEvenet += OnGameDayEvenet;
    }

    private void OnGameDayEvenet(int hour, Season season)
    {
      currentSeason = season;
    }

    private void OnAfterSceneloadEvent()
    {
        currentGrid = FindObjectOfType<Grid>();
        cropParent = GameObject.FindWithTag("CropParent").transform;
    }

    private void OnPlantSeedEvent(int id, TileDetails details)
    {
        CropDetails currentCrop = GetCropDetails(id);
        if(currentCrop != null && SeasonAvailable(currentCrop)&&details.seedItem == -1)
        {
            details.seedItem = id;
            details.growthDays = 0;
            //显示农作物
            DisplayCropPlant(details, currentCrop);
        }
        else if(details.seedItem != -1)//刷新
        {
            DisplayCropPlant(details, currentCrop);
        }

    }

    private void DisplayCropPlant(TileDetails tileDetails,CropDetails cropDetails)
    {
        int growthStages = cropDetails.growthDays.Length;
        int currentStage = 0;
        int dayCount = cropDetails.TotalGrowthDays;

        for(int i = growthStages - 1; i>=0; i--)
        {
            if (tileDetails.growthDays >= dayCount)
            {
                currentStage = i;
                break;
            }
            dayCount -= cropDetails.growthDays[i];
        }

        //赋值
        GameObject cropPrefab = cropDetails.growthPrefabs[currentStage];
        Sprite cropSprite = cropDetails.growthSprites[currentStage];

        Vector3 pos = new Vector3(tileDetails.girdX + 0.5f, tileDetails.girdY + 0.5f, 0);
        GameObject cropInstance = Instantiate(cropPrefab,pos,Quaternion.identity,cropParent);
        cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = cropSprite;
        cropInstance.GetComponent<Crop>().cropDetails = cropDetails;
    }


    //通过id查找
    public CropDetails GetCropDetails(int ID)
    {
        return cropData.cropDetailsList.Find(c=>c.seedItemID == ID);
    }

    //判断当前季节是否可以种植
    private bool SeasonAvailable(CropDetails crop)
    {
        for(int i = 0;i<crop.seasons.Length;i++)
        {
            if (crop.seasons[i] == currentSeason)
            {
                return true;
            }

        }
        return false;
    }


}
