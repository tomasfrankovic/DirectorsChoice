using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaptersManagerUI : MonoBehaviour
{
    public static ChaptersManagerUI instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning($"??? Multiple {instance} singletons");
            return;
        }
        instance = this;
    }

    public List<ChaptersButton> buttons;
    public Color selectedColor = Color.green;
    public Color deselectColor = Color.grey;


    private void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].gameObject.SetActive(false);

        for (int i = 0; i < BookManager.instance.chapterNum; i++)
            buttons[i].InitBtn(i, BookManager.instance.GetChapter(i - 1).chapterName);

        buttons[0].OnClick();
    }

    public void DeselectOthers(int id)
    {
        for (int i = 0; i < buttons.Count; i++)
            if(buttons[i].gameObject.activeSelf && i != id)
                buttons[i].OnDeselect();
    }
}
