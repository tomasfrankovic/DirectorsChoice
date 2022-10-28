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
        //thisRect.gameObject.SetActive(false);
    }

    public TextMeshProUGUI chapterBackButton;
    public TextMeshProUGUI chapterNum;
    public TextMeshProUGUI chapterTitle;
    public TextMeshProUGUI chapterText;

    public GameObject addedTextGO;
    public RectTransform thisRect;
    public RectTransform contentReset;


    public bool showedUI = false;

    public void HomeScreen()
    {
        BookManager.instance.DrawIntroduction();
        ChaptersManagerUI.instance.DeselectOthers(-1);
    }


    public void InitText(ChapterSO chapter, string page, bool reset = false, bool upatedText = false)
    {
        BookManager bookManager = BookManager.instance;
        chapterBackButton.gameObject.SetActive(page != "");
        chapterBackButton.text = $"<  {bookManager.GetChapterTitle(chapter)}";
        chapterNum.text = page;
        chapterTitle.text = bookManager.GetChapterTitle(chapter);
        chapterText.text = bookManager.GetChapterText(chapter);

        addedTextGO.SetActive(false);
        if(reset)
            contentReset.anchoredPosition = new Vector2(contentReset.anchoredPosition.x, 0f);
    }
}
