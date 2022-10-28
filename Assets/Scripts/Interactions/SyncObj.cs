using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

public abstract class SyncObj : MonoBehaviour
{
    public string uniqueId;

    public UnityEvent interactionEvent;
    public UnityEvent turnOnEvent;
    public UnityEvent turnOffEvent;

    public void MakeInteraction()
    {
        SyncManager.instance.MakeInteractions(this);
        SingleAction();
    }

    public abstract void SingleAction();
    public abstract void Interaction();


    [ContextMenu("SetEvents")]
    public void SetEvents()
    {
#if UNITY_EDITOR
        uniqueId = gameObject.name;

        turnOffEvent = new UnityEvent();
        turnOnEvent = new UnityEvent();
        UnityAction<bool> action = gameObject.SetActive;
        UnityEventTools.AddBoolPersistentListener(turnOffEvent, action, gameObject);
        UnityEventTools.RegisterBoolPersistentListener(turnOffEvent, 0, action, false);

        UnityEventTools.AddBoolPersistentListener(turnOnEvent, action, gameObject);

        if(!gameObject.GetComponent<BoxCollider2D>())
            gameObject.AddComponent<BoxCollider2D>().isTrigger = true;

        gameObject.tag = "Interactible";

        GameObject grid = GameObject.Find("Grid");
        if (grid)
            grid.GetComponent<SyncController>().LoadAllSyncedObjs();
#endif
    }

    [ContextMenu("OnEvent")]
    public void OnEvent()
    {
        turnOnEvent?.Invoke();
    }

    [ContextMenu("OffEvent")]
    public void OffEvent()
    {
        turnOffEvent?.Invoke();
    }
}
