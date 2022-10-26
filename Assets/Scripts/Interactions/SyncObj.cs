using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SyncObj : MonoBehaviour
{
    private void Start()
    {
        //register obj
        SyncManager.instance.AddSyncObj(this);
    }


    public string uniqueId;

    public UnityEvent interactionEvent;

    public void MakeInteraction()
    {
        SyncManager.instance.MakeInteractions(this);
        SingleAction();
    }

    public abstract void SingleAction();
    public abstract void Interaction();
}
