using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class BookManager : MonoBehaviour
{
    public static BookManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public BookSO bookSO;
    public SpellingWordsSO spellingWordsSO; 
    public int chapterNum;
    public int gameIncrement;
    public int chaptersUnlocked = 1;

    public List<ContentSizeFitter> fittersToReset;


    private void Start()
    {
        DrawIntroduction();
    }

    IEnumerator ResetFitters(List<ContentSizeFitter> fitters)
    {
        yield return new WaitForEndOfFrame();
        foreach (var item in fitters)
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
    }

    [ContextMenu("DrawChapters")]
    public void DrawChapter(bool reset)
    {
        TextEditorUI.instance.InitText(bookSO.chapters[chapterNum - 1], chapterNum.ToString(), reset);
        StartCoroutine(ResetFitters(fittersToReset));
    }

    [ContextMenu("DrawIntroduction")]
    public void DrawIntroduction()
    {
        TextEditorUI.instance.InitText(bookSO.introduction, "", true);
        StartCoroutine(ResetFitters(fittersToReset));
    }

    public void SelectChapter(int id)
    {
        chapterNum = id;
        DrawChapter(true);
    }

    public ChapterSO GetChapter(int id)
    {
        if (id >= bookSO.chapters.Count)
            return null;

        return bookSO.chapters[id];
    }

    public string GetChapterTitle(ChapterSO chapter)
    {
        //return bookSO.chapters[chapterNum - 1].chapterName;
        return chapter.chapterName;
    }

    public string GetChapterText(ChapterSO chapter)
    {
        List<Paragraph> paragraphs = chapter.paragraphs;
        string chapterText = "";

        for (int i = 0; i < paragraphs.Count; i++)
        {
            Paragraph paragraph = paragraphs[i];
            switch (paragraph.paragraphEnum)
            {
                case paragraphEnum.alwaysShow:
                    chapterText += GetEditedString(paragraph.paragraphText);
                    break;
                case paragraphEnum.gameIncrement:
                    if(paragraph.gameIncrement <= gameIncrement)
                        chapterText += GetEditedString(paragraph.paragraphText);
                    break;
                default:
                    break;                
            }
        }

        chapterText = chapterText.Replace("\\n", "\n").Replace("\\t", "\t");

        return chapterText;
    }

    public string GetEditedString(string str, bool addedText = false)
    {
        string editedText = str;

        List<string> atributes = GetAllAtributesStrings(str);

        for (int i = 0; i < atributes.Count; i++)
        {
            string attr = atributes[i].Substring(1, atributes[i].Length - 2);
            string replacement = $"<link=\"{attr}\"><color=\"red\"><u>{SpellingWordsManager.instance.GetGroupWord(attr)}</color></u></link>";
            editedText = editedText.Replace(atributes[i], replacement);
        }

        return editedText;
    }

    public List<string> GetAllAtributesStrings(string str)
    {
        List<string> list = new List<string>();

        foreach (Match match in Regex.Matches(str, "{[^}]+}"))
        {
            list.Add(match.Value);
        }

        return list;
    }
}
