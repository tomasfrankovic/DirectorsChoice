using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeManager : MonoBehaviour
{
    public static SceneChangeManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public CanvasGroup canvasGroup;
    bool changedScene;

    public void ChangeScene(string sceneName)
    {
        StartTransition(sceneName);
    }

    public void StartTransition(string sceneName)
    {
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, 1f).setEaseOutExpo().setOnComplete(()=> {
            canvasGroup.alpha = 1f;
            changedScene = true;
            SceneManager.LoadScene(sceneName);
        });
    }

    public void EndTransition()
    {
        if(changedScene)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.LeanAlpha(0f, 1f).setEaseOutExpo().setOnComplete(() => {
                canvasGroup.alpha = 0f;
                changedScene = true;
                canvasGroup.gameObject.SetActive(false);
            });
            changedScene = false;
        }
    }
}
