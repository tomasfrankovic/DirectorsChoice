using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookNotif : MonoBehaviour
{
    public static BookNotif instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
        gameObject.SetActive(false);
    }


    public Animator anim;


    public void Show()
    {
        gameObject.SetActive(true);
        anim.Play("bookNotif");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
