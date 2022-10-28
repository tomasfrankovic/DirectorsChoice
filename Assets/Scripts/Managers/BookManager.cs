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
    public int gameIncrement = 0;
    public int chaptersUnlocked = 1;

    public List<ContentSizeFitter> fittersToReset;


    private void Start()
    {
        //DrawIntroduction();
        SelectChapter(1);
    }

    public void UnlockNewChapter(int chapterNum)
    {
        if (chapterNum <= chaptersUnlocked)
            return;
        chaptersUnlocked = chapterNum;
        ChaptersManagerUI.instance.Init();

        BookNotif.instance.Show();
    }

    public void ChangeIncrement(int increment)
    {
        gameIncrement = increment;
        DrawChapter(false);
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
        TextEditorUI.instance.InitText(bookSO.chapters[chapterNum - 1], chapterNum.ToString());
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

    List<Paragraph> oldParagraphs = new List<Paragraph>();
    List<Paragraph> newParagraphs = new List<Paragraph>();

    public void MergeParagraphs()
    {
        foreach (var item in newParagraphs)
            oldParagraphs.Add(item);
        newParagraphs.Clear();
        BookNotif.instance.Hide();
        DrawChapter(false);
        TextEditorUI.instance.ShowUpdatedText(false);
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
                    chapterText += GetEditedString(paragraph.paragraphText, CheckNewParagraph(paragraph));
                    break;
                case paragraphEnum.gameIncrement:
                    if(paragraph.gameIncrement <= gameIncrement)
                        chapterText += GetEditedString(paragraph.paragraphText, CheckNewParagraph(paragraph));
                    break;
                default:
                    break;                
            }
        }

        chapterText = chapterText.Replace("\\n", "\n").Replace("\\t", "\t");

        return chapterText;
    }

    bool CheckNewParagraph(Paragraph paragraph)
    {
        if (!oldParagraphs.Contains(paragraph))
        {
            if (!newParagraphs.Contains(paragraph))
            {
                newParagraphs.Add(paragraph);
                BookNotif.instance.Show();
                TextEditorUI.instance.ShowUpdatedText(true);
            }
            
            return true;
        }
        return false;
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
        if (addedText)
            editedText = $"<mark=#04d40f70>{editedText}</mark>";

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
