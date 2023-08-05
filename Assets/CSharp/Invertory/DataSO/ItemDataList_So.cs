using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataList_So", menuName = "Invertory/ItemDataList")]
public class ItemDataList_So : ScriptableObject
{
    public List<ItemDetails> itemDataList;
}
