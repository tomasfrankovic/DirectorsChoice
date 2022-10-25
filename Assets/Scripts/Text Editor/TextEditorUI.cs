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
