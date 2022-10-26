using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncManager : MonoBehaviour
{
    public static SyncManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }



    public Dictionary<string, List<SyncObj>> syncedObjs = new Dictionary<string, List<SyncObj>>();

    public void MakeInteractions(SyncObj syncObj)
    {
        if (!syncedObjs.ContainsKey(syncObj.uniqueId))
            Debug.LogWarning("Synced list doesn't contain: " + syncObj.uniqueId);
        else
            for (int i = 0; i < syncedObjs[syncObj.uniqueId].Count; i++)
                syncedObjs[syncObj.uniqueId][i].Interaction();
    }

    public void AddSyncObj(SyncObj obj)
    {
        if (!syncedObjs.ContainsKey(obj.uniqueId))
            syncedObjs.Add(obj.uniqueId, new List<SyncObj>() { obj });
        else
            syncedObjs[obj.uniqueId].Add(obj);
    }
}
