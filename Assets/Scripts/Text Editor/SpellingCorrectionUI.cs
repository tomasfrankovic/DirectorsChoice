using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellingCorrectionUI : MonoBehaviour, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public static SpellingCorrectionUI instance;    
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public CanvasGroup canvasGroup;
    public RectTransform thisRect;
    public RectTransform childRect;
    public float offset = 20f;

    public List<SpellingOptions> spellingOptions;

    public List<ContentSizeFitter> fitters;

    public float tweenTime = 1f;



    private Vector2 savedPos = Vector2.negativeInfinity;
    private void Start()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }


    public void InitSpellingCorection(Vector2 pos, string key)
    {
        if (pos == savedPos)
            return;

        SetOptions(key);
        StartCoroutine(ResetFitters(fitters, pos));
    }

    void SetOptions(string key)
    {
        List<spellingWords> words = SpellingWordsManager.instance.GetWordsList(key);

        for (int i = 0; i < spellingOptions.Count; i++)
            spellingOptions[i].gameObject.SetActive(false);

        for (int i = 0; i < words.Count; i++)
            spellingOptions[i].Init(words[i], key);
    }

    IEnumerator ResetFitters(List<ContentSizeFitter> fitters, Vector2 pos)
    {        
        yield return new WaitForEndOfFrame();
        foreach (var item in fitters)
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);

        SetWordChanger(pos);
    }

    void SetWordChanger(Vector2 pos)
    {
        savedPos = pos;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
        transform.position = pos;

        float yValue = Screen.currentResolution.height / 2 < pos.y ? -offset : offset;
        thisRect.pivot = new Vector2(thisRect.pivot.x, Screen.currentResolution.height / 2 < pos.y ? 1f : 0f);

        //Debug.Log($"pos: {pos}, yValue: {yValue}");
        thisRect.anchoredPosition = new Vector2(thisRect.anchoredPosition.x, thisRect.anchoredPosition.y + yValue);

        LeanTween.cancel(gameObject);

        thisRect.localScale = Vector3.one * 0.5f;
        canvasGroup.alpha = 0f;
        thisRect.LeanScale(Vector3.one, tweenTime).setEaseOutExpo();
        canvasGroup.LeanAlpha(1f, tweenTime /2f).setEaseOutExpo();
    }

    public void Idk()
    {
        Debug.Log("Test");
    }

    public void CloseUI()
    {
        thisRect.LeanScale(Vector3.zero, tweenTime).setEaseOutExpo();
        canvasGroup.LeanAlpha(0f, tweenTime).setEaseOutExpo();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        savedPos = Vector2.negativeInfinity;
    }


    private bool mouseIsOver = false;
    public void OnDeselect(BaseEventData eventData)
    {
        if (!mouseIsOver)
            CloseUI();              
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
