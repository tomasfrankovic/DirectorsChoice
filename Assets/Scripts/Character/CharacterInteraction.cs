using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{

    public List<Collider2D> collisions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Interactible"))
            collisions.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactible"))
            collisions.Remove(collision);
    }

    public void MakeInteractions()
    {
        if (collisions.Count == 0)
            Debug.LogWarning("No colliders to interact with");

        for (int i = 0; i < collisions.Count; i++)
        {
            SyncObj obj;
            if(obj = collisions[i].GetComponent<SyncObj>())
                obj.MakeInteraction();
        }
    }
}
