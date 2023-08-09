using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "MapData_SO", menuName = "Map/MapData_SO")]
public class MapData_SO : ScriptableObject
{
    [SceneName]public string sceneName;

    public List<TileProperty> tileProperties;
}

