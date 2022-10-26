using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEditorUI : MonoBehaviour
{
    public static TextEditorUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public TextMeshProUGUI chapterBackButton;
    public TextMeshProUGUI chapterNum;
    public TextMeshProUGUI chapterTitle;
    public TextMeshProUGUI chapterText;

    public GameObject addedTextGO;
    public RectTransform thisRect;

    [ContextMenu("Show")]
    public void Show()
    {
        LeanTween.cancel(thisRect);
        thisRect.LeanMoveLocalY(0f, 1f).setEaseOutExpo();
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        LeanTween.cancel(thisRect);
        thisRect.LeanMoveLocalY(-1500f, 1f).setEaseOutExpo();
    }

    public void InitText()
    {
        BookManager bookManager = BookManager.instance;
        chapterBackButton.text = $"<  {bookManager.GetChapterTitle()}";
        chapterNum.text = bookManager.GetChapterNum();
        chapterTitle.text = bookManager.GetChapterTitle();
        chapterText.text = bookManager.GetChapterText();

        addedTextGO.SetActive(false);
    }
}
