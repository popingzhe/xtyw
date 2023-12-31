using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Seed,Commodity,Furniture,
    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,
    ReapableScenery,defalut
}

public enum SlotType
{
    Bag,Box,Shop
}

public enum InvertoryLocation
{
    Bag,Box, Shop
}

public enum PartType
{
    None,Carry,Hoe,Break,Water,Collect,Chop
}

public enum PartName
{
    Body,Hair,Arm,Tool
}

public enum GridType
{
    Diggable,DropItem,PlaceFurniture,NPCObstacle
}

public enum Season
{
   spring,summer, autumn,winter
}

public enum ParticaleEffecttype
{
    None,LeavesFalling,Rock,ReapableScenery
}