using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncController : MonoBehaviour
{
    public List<SyncObj> syncedObjs = new List<SyncObj>();

    [ContextMenu("LoadAllSyncedObjs")]
    public void LoadAllSyncedObjs()
    {
        syncedObjs = new List<SyncObj>();
        CheckChildren(transform);
    }

    public void CheckChildren(Transform trans)
    {
        foreach (Transform child in trans)
        {
            if (child.GetComponent<SyncObj>())
                syncedObjs.Add(child.GetComponent<SyncObj>());
            if (child.childCount > 0)
                CheckChildren(child);
        }
            
    }
    public void AddToManager()
    {
        foreach (var item in syncedObjs)
            SyncManager.instance.AddSyncObj(item);
    }
}
