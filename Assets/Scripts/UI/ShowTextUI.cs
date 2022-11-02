using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class ShowTextUI : MonoBehaviour
{
    public static ShowTextUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public enum textShowState
    {
        none,
        showing,
        showed,
    }

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI choicesText;

    public float timeLapse = 0.01f;

    public textShowState actualState;

    public RectTransform thisRect;

    TextMeshProUGUI actualTMP;
    string actualText;

    Action yesCallback;
    Action noCallback;
    Action onFinish;

    private void Start()
    {
        thisRect.gameObject.SetActive(false);
    }

    bool saveCallback;
    public void ShowChoiceText(string text, Action yesCallback, Action noCallback, bool saveCallback = true)
    {
        if (AbstractRoomLogic.instance.simulationRunning)
        {
            if (AbstractRoomLogic.instance.lastInteraction.answer)
                yesCallback?.Invoke();
            return;
        }

        this.saveCallback = saveCallback;
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;
        RunText(choicesText, text);
    }

    public void ShowMainText(string text, Action onFinish = null)
    {
        if (AbstractRoomLogic.instance.simulationRunning) 
        {
            onFinish?.Invoke();
            return;
        }
        this.onFinish = onFinish;
        RunText(mainText, text);
    }

    public bool CanMove()
    {
        return actualState == textShowState.none;
    }
    
    [ContextMenu("TestText")]
    public void TestText()
    {
        ShowMainText("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's stan  the industry's stan ");
    }

    [ContextMenu("TestChoiceText")]
    public void TestChoiceText()
    {
        ShowChoiceText("Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has", () => { ShowMainText("Klikol si 'yes' retard"); }, () => { ShowMainText("Klikol si 'no' retard"); });
    }

    [ContextMenu("SkipText")]
    public void SkipText()
    {
        StopAllCoroutines();
        if(actualState == textShowState.showing)
        {
            actualTMP.text = actualText;
            actualState = textShowState.showed;

            LeanTween.cancel(thisRect);
            thisRect.sizeDelta = new Vector2(980f, 235f);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else if (actualState == textShowState.showed && actualTMP != choicesText)
        {
            ClearText();
            if(onFinish != null)
                onFinish.Invoke();
        }
    }

    public void RunText(TextMeshProUGUI textComponent, string text)
    {
        LeanTween.cancel(thisRect);
        thisRect.LeanSize(new Vector2(980f, 235f), 0.5f).setEaseOutCubic();
        thisRect.gameObject.SetActive(true);
        StopAllCoroutines();
        choicesText.gameObject.SetActive(textComponent == choicesText);
        mainText.gameObject.SetActive(textComponent == mainText);
        actualTMP = textComponent;
        actualText = text;
        textComponent.text = "";
        actualState = textShowState.showing;

        StartCoroutine(BuildText());
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator BuildText()
    {
        for (int i = 0; i < actualText.Length; i++)
        {
            actualTMP.text = string.Concat(actualTMP.text, actualText[i]);
            //Wait a certain amount of time, then continue with the for loop
            yield return new WaitForSeconds(timeLapse);
        }
        actualState = textShowState.showed;
    }

    void ClearText()
    {
        actualState = textShowState.none;
        LeanTween.cancel(thisRect);
        thisRect.LeanSize(new Vector2(0f, 235f), .5f).setEaseInCubic().setOnComplete(()=> {
            thisRect.gameObject.SetActive(false);
            mainText.text = "";            
            choicesText.gameObject.SetActive(false);
        });
    }

    public void ClickYes()
    {
        if (saveCallback)
            AbstractRoomLogic.instance.lastInteraction.answer = true;
        ClearText();
        yesCallback?.Invoke();
    }

    public void ClickNo()
    {
        ClearText();
        noCallback?.Invoke();
    }
}
