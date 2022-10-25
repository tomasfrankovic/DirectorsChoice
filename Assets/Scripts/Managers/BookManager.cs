using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

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


    [ContextMenu("DrawChapters")]
    public void DrawChapter()
    {
        TextEditorUI.instance.InitText();
    }

    public string GetChapterNum()
    {
        return chapterNum.ToString();
    }

    public string GetChapterTitle()
    {
        return bookSO.chapters[chapterNum - 1].chapterName;
    }

    public string GetChapterText()
    {
        List<Paragraph> paragraphs = bookSO.chapters[chapterNum - 1].paragraphs;
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
            editedText = str.Replace(atributes[i], replacement);
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
