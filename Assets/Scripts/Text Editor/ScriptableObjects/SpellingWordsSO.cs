using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum spellingWords
{
    locked,
    unlocked,
}

[CreateAssetMenu(fileName = "SpellingWordsSO", menuName = "ScriptableObjects/SpellingWordsSO", order = 1)]
public class SpellingWordsSO : SerializedScriptableObject 
{
    public Dictionary<string, List<spellingWords>> wordGroups = new Dictionary<string, List<spellingWords>>();
}