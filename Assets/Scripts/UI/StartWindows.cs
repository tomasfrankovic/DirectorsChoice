using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartWindows : MonoBehaviour
{
    public static StartWindows instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public Animator anim;
    public CanvasGroup canvasGroup;
    public CanvasGroup blockImg;
    public CanvasGroup whiteBacklight;
    public bool animLock;
    public bool showedUI = false;

    public GameObject desktop;
    public GameObject word;

    private void Start()
    {
        gameObject.transform.localPosition = new Vector3(0f, -300f);
        gameObject.SetActive(true);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        Show();
    }

    public void ShowDesktop()
    {
        desktop.SetActive(true);
        word.SetActive(false);
    }

    public void ShowWord()
    {
        if (OnboardingManager.instance != null)
            OnboardingManager.instance.OpenedWord();

        desktop.SetActive(false);
        word.SetActive(true);
    }

    public bool IsShowedUI()
    {
        return animLock || showedUI;
    }

    public void TextEditorClicked()
    {
        if (OnboardingManager.instance != null)
        {
            if (OnboardingManager.onboardingIndex == 0)
                return;
            else if (OnboardingManager.onboardingIndex == 1)
                OnboardingManager.instance.NtbInteracted();
        }
            

        if (showedUI)
            Hide();
        else
            Show();
    }

    public void Show()
    {
        showedUI = true;
        TweenShowLaptop();
    }

    public void Hide()
    {
        showedUI = false;
        HideScreen();
        BookManager.instance.MergeParagraphs();
    }

    public void TweenHideLaptop()
    {
        CancelTween();

        gameObject.LeanMoveLocalY(-300f, .5f).setEaseInExpo().setOnComplete(() => {
            animLock = false;
        }); ;
    }

    public void TweenShowLaptop()
    {
        CancelTween();

        animLock = true;

        gameObject.LeanMoveLocalY(0f, .5f).setEaseOutExpo().setOnComplete(() => {
            anim.Play("ntb_open");
        });
    }

    public void ShowScreen()
    {
        CancelTween();

        //canvasGroup.LeanAlpha(1f, 1f).setDelay(.2f).setEaseOutCirc().setOnComplete(() => {
        whiteBacklight.alpha = 1f;
        canvasGroup.alpha = 1f;
        whiteBacklight.LeanAlpha(0f, 1f).setDelay(.2f).setEaseOutCirc().setOnComplete(() => {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            animLock = false;
        });
        blockImg.LeanAlpha(1f, 1f).setEaseOutCirc();
    }

    public void HideScreen()
    {
        CancelTween();

        animLock = true;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.LeanAlpha(0f, .5f).setOnComplete(() => {

            anim.Play("ntb_close");
            canvasGroup.alpha = 0f;
        });
        blockImg.LeanAlpha(0f, .5f).setEaseOutCirc();
    }

    void CancelTween()
    {
        LeanTween.cancel(canvasGroup.gameObject);
        LeanTween.cancel(gameObject);
        LeanTween.cancel(blockImg.gameObject);
    }
}
