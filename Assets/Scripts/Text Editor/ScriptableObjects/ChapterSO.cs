using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum paragraphEnum
{
    alwaysShow,
    gameIncrement,

}

[Serializable]
public class Paragraph
{
    public paragraphEnum paragraphEnum;

    [ShowIf("paragraphEnum", paragraphEnum.gameIncrement)]
    public int gameIncrement;

    [TextArea(3, 30)]
    public string paragraphText;
}

[InlineEditor]
[CreateAssetMenu(fileName = "ChapterSO", menuName = "ScriptableObjects/ChapterSO", order = 1)]
public class ChapterSO : ScriptableObject
{
    public string chapterName;
    public List<Paragraph> paragraphs;
}