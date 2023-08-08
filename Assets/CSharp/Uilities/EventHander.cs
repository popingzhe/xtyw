using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EventHander
{
    public static event Action<InvertoryLocation, List<InvertoryItem>> UpdateInvertoryUI;

    public static void CallUpdateInvertoryUI(InvertoryLocation location,List<InvertoryItem> items)
    {
        UpdateInvertoryUI?.Invoke(location, items);
    }

    public static event Action<int, Vector3> InstantiateiyemInSence;
    public static void CallInstantiateiyemInSence(int ID,Vector3 pos)
    {
        InstantiateiyemInSence?.Invoke(ID, pos);
    }

    public static event Action<ItemDetails, bool> ItemSelectEvent;
    public static void CallItemSelectEvent(ItemDetails itemDetails,bool isSelected) 
    { 
        ItemSelectEvent?.Invoke(itemDetails, isSelected);
    }

    public static event Action<String, string, Vector3> TransitionEvent;
    public static void CallTransitionEvent(String s,string name,Vector3 pos)
    {
        TransitionEvent?.Invoke(s,name, pos);
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
}
