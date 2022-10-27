using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractRoomLogic : MonoBehaviour
{
    public static AbstractRoomLogic instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public abstract void WordChanged(spellingWords word);
    public abstract void InteractionHappened(string interaction);
    public abstract void Init();

    public bool BookContains(spellingWords word)
    {
        return SpellingWordsManager.instance.ContainsWord(word);
    }

    public bool IsItemSelected(inventoryItems item)
    {
        return InventoryManager.instance.selectedItem == item;
    }
}
