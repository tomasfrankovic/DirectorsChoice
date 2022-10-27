using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum inventoryItems
{
    none,
    valve,
    screwdriver,
    key,
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }
 
    public InventoryItemsSO InventoryItemsSO;
    public List<inventoryItems> storedItems;
    public inventoryItems selectedItem;

    public List<InventorySlot> slots;
    public InventorySlot usingSlot;

    private void Start()
    {
        storedItems.Clear();
        UpdateUI();
    }

    public void SelectItem(inventoryItems item)
    {
        selectedItem = item;
        UpdateUI();
    }

    public void DeselectItem()
    {
        selectedItem = inventoryItems.none;
        UpdateUI();
    }

    public void AddItemToInventory(inventoryItems item)
    {
        if (storedItems.Contains(item)) return;

        storedItems.Add(item);

        UpdateUI();
    }   

    public void RemoveItemFromInventory(inventoryItems item)
    {
        if (storedItems.Contains(item))
            storedItems.Remove(item);
        else
            Debug.LogWarning("Stored items doesn't contain " + item);

        if(item == selectedItem)
            selectedItem = inventoryItems.none;

        UpdateUI();
    }

    public void ChangeItemInInventory(inventoryItems itemToChange, inventoryItems item)
    {
        if (storedItems.Contains(itemToChange))
            storedItems.Remove(itemToChange);
        else
            Debug.LogWarning("Stored items doesn't contain " + itemToChange);

        if (!storedItems.Contains(item))
            storedItems.Add(item);

        UpdateUI();
    }


    public void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
            if(storedItems.Count <= i)
                slots[i].Init(inventoryItems.none);
            else
                slots[i].Init(storedItems[i], selectedItem == storedItems[i]);

        usingSlot.Init(selectedItem, true);
    }
}