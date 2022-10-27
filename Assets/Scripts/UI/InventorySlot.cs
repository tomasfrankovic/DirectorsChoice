using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject selectBorder;
    public GameObject background;
    public Image img;
    public Button btn;

    public inventoryItems item = inventoryItems.none;

    bool changed;
    bool selected;
    public void Init(inventoryItems item, bool selected = false)
    {
        this.selected = selected;
        changed = item != this.item;
        this.item = item;
        background.SetActive(item != inventoryItems.none);
        selectBorder.SetActive(item != inventoryItems.none && selected);
        btn.interactable = (item != inventoryItems.none);
        img.gameObject.SetActive(item != inventoryItems.none);
        img.sprite = InventoryManager.instance.InventoryItemsSO.GetSprite(item);
        if (changed)
        {
            img.transform.localScale = Vector3.zero;
            img.transform.LeanScale(Vector3.one, .8f).setEaseOutBack();
        }
    }


    public void OnClick()
    {
        if(selected)
            InventoryManager.instance.DeselectItem();
        else
            InventoryManager.instance.SelectItem(item);
    }
}
