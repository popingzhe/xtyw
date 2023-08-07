using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InertoryBag_SO", menuName = "Invertory/InertoryBag_SO")]
public class InertoryBag_SO : ScriptableObject
{
    public List<InvertoryItem> itemList;
}
