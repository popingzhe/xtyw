using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EventHander
{
    public static event Action<InvertoryLocation, List<InvertoryItem>> UpdateInvertoryUI;

    public static void CallUpdateInvertoryUI(InvertoryLocation location, List<InvertoryItem> items)
    {
        UpdateInvertoryUI?.Invoke(location, items);
    }

    public static event Action<int, Vector3> InstantiateiyemInSence;
    public static void CallInstantiateiyemInSence(int ID, Vector3 pos)
    {
        InstantiateiyemInSence?.Invoke(ID, pos);
    }

    //扔东西
    public static event Action<int, Vector3,ItemType> DropItemEvent;
    public static void CallDropItemEvent(int ID, Vector3 pos,ItemType itemType)
    {
        DropItemEvent?.Invoke(ID, pos,itemType);
    }


    public static event Action<ItemDetails, bool> ItemSelectEvent;
    public static void CallItemSelectEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<String, string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(String s, string name, Vector3 pos)
    {
        TransitionEvent?.Invoke(s, name, pos);
    }

    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    public static event Action AfterSceneloadEvent;
    public static void CallAfterSceneloadEvent()
    {
        AfterSceneloadEvent?.Invoke();
    }

    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 pos)
    {
        MoveToPosition?.Invoke(pos);
    }

    public static event Action<Vector3, ItemDetails> MouseClickedEvent;
    public static void CallMouseClickedEvent(Vector3 pos, ItemDetails itemDetails)
    {
        MouseClickedEvent?.Invoke(pos, itemDetails);
    }

    public static event Action<Vector3, ItemDetails> ExecuteActionAfterAnimation;
    public static void CallExecuteActionAfterAnimation(Vector3 pos, ItemDetails itemDetails)
    {
         ExecuteActionAfterAnimation?.Invoke(pos, itemDetails);
    }

    //时间事件
    public static event Action<int, int> GameMinuteEvenet;
    public static void CallGameMinuteEvenet(int minute,int hour)
    {
        GameMinuteEvenet?.Invoke(minute, hour);
    }

    public static event Action<int, int, int, int,Season> GameDateEvenet;
    public static void CallGameDateEvenet(int hour,int day,int month,int year,Season season)
    {
        GameDateEvenet?.Invoke(hour,day,month,year,season);
    }

    public static event Action<int, Season> GameDayEvenet;
    public static void CallGameDayEvenet(int day,Season season)
    {
        GameDayEvenet?.Invoke(day,season);
    }

    public static event Action<int, TileDetails> PlantSeedEvent;
    public static void CallPlantSeedEvent(int ID,TileDetails tile)
    {
        PlantSeedEvent?.Invoke(ID,tile);
    }

    public static Action<int> HarvestAtPlayerPosition;

    public static void CallHarvestAtPlayerPosition(int ID)
    {
        HarvestAtPlayerPosition?.Invoke(ID);
    }

    public static Action RefreshCurrentMap;

    public static void CallRefreshCurrentMap()
    {
        RefreshCurrentMap?.Invoke();
    }
}

