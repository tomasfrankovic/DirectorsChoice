using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{

    public List<Collider2D> collisions;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Scare"))
        {
            ShowTextUI.instance.ShowMainText("You’d rather not be in the dark.");
        }
        if(collision.CompareTag("Interactible"))
        {
            if (OnboardingManager.onboardingIndex == 0 && OnboardingManager.instance != null && collision.name == "door_tuto")
            {
                OnboardingManager.instance.ShowAction(true);
                collisions.Add(collision);
            }
            else
                collisions.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactible"))
        {
            if (OnboardingManager.onboardingIndex == 0 && OnboardingManager.instance != null && collision.name == "door_tuto")
            {
                OnboardingManager.instance.ShowAction(false);
                collisions.Remove(collision);
            }
            else
                collisions.Remove(collision);
        }
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
