using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookSO", menuName = "ScriptableObjects/BookSO", order = 1)]
public class BookSO : ScriptableObject
{
    public ChapterSO introduction;
    public List<ChapterSO> chapters;
}