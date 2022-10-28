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
    soap_bars,
    rotary_telephones,
    sofa,
    pile_of_rocks,
    toilet,
    plumbing,
    wall_hydrants,
    windows,
    toilets,
    chromic_scarfs,
    warm,
    cold,
}

[CreateAssetMenu(fileName = "SpellingWordsSO", menuName = "ScriptableObjects/SpellingWordsSO", order = 1)]
public class SpellingWordsSO : SerializedScriptableObject 
{
    public Dictionary<string, List<spellingWords>> wordGroups = new Dictionary<string, List<spellingWords>>();
}