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

    public void TextEditorClicked()
    {
        if (showedUI)
            Hide();
        else
            Show();
    }

    [ContextMenu("Show")]
    public void Show()
    {
        LeanTween.cancel(thisRect);
        if (!thisRect.gameObject.activeSelf)
        {
            thisRect.gameObject.SetActive(true);
            thisRect.transform.localPosition = new Vector3(0f, -1500f);
        }

        thisRect.LeanMoveLocalY(0f, 1f).setEaseOutExpo();
        showedUI = true;
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        LeanTween.cancel(thisRect);
        thisRect.LeanMoveLocalY(-1500f, 1f).setEaseOutExpo();
        showedUI = false;
    }

    public void InitText(ChapterSO chapter, string page, bool reset = false)
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
