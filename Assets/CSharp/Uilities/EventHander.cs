using System;
using System.Collections;
using System.Collections.Generic;
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
}
