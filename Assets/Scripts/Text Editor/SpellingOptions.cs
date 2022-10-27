using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellingOptions : MonoBehaviour
{
    public TextMeshProUGUI text;
    public spellingWords spellingWord;
    public string key;

    public void Init(spellingWords spellingWord, string key)
    {
        gameObject.SetActive(true);
        this.key = key;
        this.spellingWord = spellingWord;
        text.text = spellingWord.ToString().Replace("_", " ");
    }

    public void OnSelect()
    {
        SpellingWordsManager.instance.ChangeWord(key, spellingWord);
        AbstractRoomLogic.instance.WordChanged(spellingWord);
        BookManager.instance.DrawChapter(false);
        SpellingCorrectionUI.instance.CloseUI();
    }
}
