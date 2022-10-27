using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum spellingWords
{
    lonely,
    bright,
    fluffy,
    furry,
    sleepy,
    only,
    wooden,
    metal_safes,
    dog_food,
    rotary_telephones,
    sofa,
    pile_of_rocks,
    toilet,
}

[CreateAssetMenu(fileName = "SpellingWordsSO", menuName = "ScriptableObjects/SpellingWordsSO", order = 1)]
public class SpellingWordsSO : SerializedScriptableObject 
{
    public Dictionary<string, List<spellingWords>> wordGroups = new Dictionary<string, List<spellingWords>>();
}