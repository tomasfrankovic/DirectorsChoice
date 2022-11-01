using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class OnboardingManager : MonoBehaviour
{
    public static OnboardingManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
        //thisRect.gameObject.SetActive(false);
    }

    public static int onboardingIndex = 0;
    public GameObject inventory;

    public RectTransform arrows;
    public RectTransform space;
    public RectTransform action;

    private void Start()
    {
        arrows.gameObject.SetActive(false);
        space.gameObject.SetActive(false);
        action.gameObject.SetActive(false);
    }

    public void OpenedWord()
    {
        if (onboardingIndex != 0)
            return;

        Debug.Log("Opened word");
        Wait(1f, () =>
        {
            StartWindows.instance.Hide();
            InventoryManager.instance.gameObject.SetActive(false);

            Wait(1f, () =>
            {
            ShowTextUI.instance.ShowMainText("You feel parched after hours of tenuous work. Maybe you should get some water from the kitchen.", () => { MoveIn(arrows); });
            });
        });
    }

    public async void Wait(float time, Action callback)
    {
        TimersManager.instance.AddTimer(time, callback, false);
        //await Task.Delay(Mathf.RoundToInt(time * 1000f));
        //callback?.Invoke();
    }

    public void MoveIn(RectTransform rect)
    {
        LeanTween.cancel(rect.gameObject);
        rect.gameObject.SetActive(true);

        rect.anchoredPosition = Vector2.down * 300f;

        rect.LeanAnchoredPos(Vector2.zero, 1f).setEaseOutBack();
    }

    public void MoveOut(RectTransform rect)
    {
        LeanTween.cancel(rect.gameObject);
        rect.gameObject.SetActive(true);

        //rect.anchoredPosition = Vector2.down * 100f;

        rect.LeanAnchoredPos(Vector2.down * 300f, 1f).setEaseOutExpo();
    }

    public void ShowAction(bool show)
    {
        if (show)
            MoveIn(action);
        else
            MoveOut(action);
    }

    public void ShowSpace(bool show)
    {
        if (show)
        {
            BookManager.instance.ChangeIncrement(1);
            MoveIn(space);
        }
        else
            MoveOut(space);
    }

    public void DoorInteracted()
    {
        onboardingIndex++;
        MoveOut(arrows);
        ShowAction(false);
        SoundManager.instance.PlayMusic("music");
    }

    public void NtbInteracted()
    {
        onboardingIndex++;

        Wait(1f, () =>
        {
            InventoryManager.instance.gameObject.SetActive(true);

        });
    }
}
