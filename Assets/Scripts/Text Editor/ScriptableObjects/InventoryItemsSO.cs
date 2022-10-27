using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemsSO", menuName = "ScriptableObjects/InventoryItemsSO", order = 1)]
public class InventoryItemsSO : SerializedScriptableObject
{
    public Dictionary<inventoryItems, Sprite> wordGroups = new Dictionary<inventoryItems, Sprite>();

    public Sprite GetSprite(inventoryItems item)
    {
        if (wordGroups.ContainsKey(item))
            return wordGroups[item];
        Debug.LogWarning("wordGroups don't contain: " + wordGroups);
        return null;
    }
}
