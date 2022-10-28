using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneUI : MonoBehaviour
{
    public static CutsceneUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public bool isCutscene = false;
    public CanvasGroup canvasGroup;

    public float fadeTime = 1f;
    public float unfadeTime = 1f;

    public LeanTweenType fadeEase = LeanTweenType.easeOutExpo;
    public LeanTweenType unfadeEase = LeanTweenType.easeOutExpo;

    [ContextMenu("Test")]
    public void Test()
    {
        ShowCutScene(0f, ()=> { });
    }

    public void ShowCutScene(float delay, Action middleCallback)
    {
        if (AbstractRoomLogic.instance.simulationRunning)
        {
            middleCallback?.Invoke();
            return;
        }
        isCutscene = true;
        LeanTween.alphaCanvas(canvasGroup, 1f, fadeTime).setEase(fadeEase).setOnComplete(()=> {
            LeanTween.alphaCanvas(canvasGroup, 0f, unfadeTime).setDelay(delay).setEase(unfadeEase).setOnStart(()=> { middleCallback?.Invoke(); }).setOnComplete(() => {
                isCutscene = false;
            });
        });
    }
}
