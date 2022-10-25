using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellingWordsManager : MonoBehaviour
{
    public static SpellingWordsManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public SpellingWordsSO spellingWordsSO;
    public Dictionary<string, spellingWords> selectedWords;
    public List<spellingWords> selectedWordsList;

    private void Start()
    {
        Init();
    }
    public void Init()
    {
        selectedWords = new Dictionary<string, spellingWords>();
        selectedWordsList = new List<spellingWords>();
        foreach (var item in spellingWordsSO.wordGroups)
        {
            selectedWords.Add(item.Key, item.Value[0]);
            selectedWordsList.Add(item.Value[0]);
        }

    }

    public List<spellingWords> GetWordsList(string key)
    {
        if (spellingWordsSO.wordGroups.ContainsKey(key))
            return spellingWordsSO.wordGroups[key];
        return new List<spellingWords>();
    }

    public string GetGroupWord(string key)
    {
        if (selectedWords.ContainsKey(key))
            return selectedWords[key].ToString();
        else
            Debug.LogWarning("selectedWords doesn't contain key");
        return "";
    }

    public void ChangeWord(string key, spellingWords wordEnum)
    {
        if (selectedWords.ContainsKey(key))
        {
            selectedWords[key] = wordEnum;
            selectedWordsList = new List<spellingWords>();
            foreach (var item in selectedWords)
                selectedWordsList.Add(item.Value);
        }
        else
            Debug.LogWarning("selectedWords doesn't contain key");
    }
}
