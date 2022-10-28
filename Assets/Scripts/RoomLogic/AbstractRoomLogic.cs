using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionHappened
{
    public List<spellingWords> words;
    public string interaction = "";
    public spellingWords word = 0;
    public inventoryItems itemUsed;
    public bool answer = false;

    public InteractionHappened(string interaction)
    {
        words = new List<spellingWords>(SpellingWordsManager.instance.selectedWordsList);
        this.interaction = interaction;
        itemUsed = InventoryManager.instance.selectedItem;
    }

    public InteractionHappened(spellingWords word)
    {
        words = new List<spellingWords>(SpellingWordsManager.instance.selectedWordsList);
        this.word = word;
        itemUsed = InventoryManager.instance.selectedItem;
    }

    public void ReplayInteraction()
    {
        SpellingWordsManager.instance.selectedWordsList = words;
        InventoryManager.instance.selectedItem = itemUsed;

        if (interaction != "")
            AbstractRoomLogic.instance.InteractionHappened(interaction);
        else
            AbstractRoomLogic.instance.WordChanged(word);

        //Debug.Log($"Replay: {interaction}, {itemUsed}");
    }
}

public abstract class AbstractRoomLogic : MonoBehaviour
{
    public static AbstractRoomLogic instance;

    public static Dictionary<string, List<InteractionHappened>> roomsHistory = new Dictionary<string, List<InteractionHappened>>();
    public static bool dictInited = false;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public virtual void WordChanged(spellingWords word, bool important = true)
    {
        if (important && !simulationRunning)
            SaveInteraction(new InteractionHappened(word));
    }

    public virtual void InteractionHappened(string interaction, bool important = true)
    {
        if (important && !simulationRunning)
            SaveInteraction(new InteractionHappened(interaction));
    }

    public InteractionHappened lastInteraction;
    public void SaveInteraction(InteractionHappened interaction)
    {
        if (!roomsHistory.ContainsKey(SceneManager.GetActiveScene().name))
            roomsHistory.Add(SceneManager.GetActiveScene().name, new List<InteractionHappened>());
        roomsHistory[SceneManager.GetActiveScene().name].Add(interaction);
        lastInteraction = interaction;
    }

    public abstract void Init();

    public bool IsWordSelected(spellingWords word)
    {
        return SpellingWordsManager.instance.ContainsWord(word);
    }

    public bool IsItemSelected(inventoryItems item)
    {
        return InventoryManager.instance.selectedItem == item;
    }

    public bool simulationRunning;
    public void RunRoomSimulation()
    {
        simulationRunning = true;
        string sceneName = SceneManager.GetActiveScene().name;

        if(roomsHistory.ContainsKey(SceneManager.GetActiveScene().name))
        {
            List<InteractionHappened> interactions = roomsHistory[SceneManager.GetActiveScene().name];

            List<spellingWords>  savedWords = new List<spellingWords>(SpellingWordsManager.instance.selectedWordsList);
            inventoryItems savedItemUsed = InventoryManager.instance.selectedItem;

            foreach (var item in interactions)
            {
                lastInteraction = item;
                item.ReplayInteraction();
            }
        }
        simulationRunning = false;
    }

    [ContextMenu("Restart")]
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
