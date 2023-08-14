using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails 
{
    public int seedItemID;
    [Header("��ͬ�׶���Ҫ����")]
    public int[] growthDays;
    public int TotalGrowthDays;

    [Header("��ͬ�׶�������prefab")]
    public GameObject[] growthPrefabs;

    [Header("��ͬ�׶ε�ͼƬ")]
    public Sprite[] growthSprites;

    [Header("����ֲ�ļ���")]
    public Season[] seasons;

    [Space]
    [Header("�ո��")]
    public int[] harvestToolItemID;

    [Header("ÿ�ֹ���ʹ�ô���")]
    public int[] requirActionCount;

    [Header("ת����ƷID")]
    public int transferItemID;
    [Space]
    [Header("�ո��ʵ��Ϣ")]
    public int[] producedItemID;
    public int[] producedMinAmount;
    public int[] producedMaxAmount;
    public Vector2 spwanRadius;

    [Header("�ٴ�����ʱ��")]
    public int daysToRegrow;
    public int regrowTimes;

    [Header("Options")]
    public bool generateAtPlayerPosition;

    public bool hasAimation;

    public bool hasParticalEffect;
    //todo


    //�����Ƿ����
    public bool CheckToolAvailable(int toolID)
    {
        foreach (var tool in harvestToolItemID)
        {
            if(tool == toolID)
                return true;
        }
        return false;
    }

    //��ȡ����ʹ�ô���
    public int GetTotalRequireCount(int toolID)
    {
        for (int i = 0; i < harvestToolItemID.Length; i++)
        {
            if (harvestToolItemID[i] == toolID)
                return requirActionCount[i];
        }
        return -1;

    }

}
