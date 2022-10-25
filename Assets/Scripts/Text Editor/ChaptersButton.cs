using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChaptersButton : MonoBehaviour
{
    public Button button;
    public GameObject buttonSelectImg;
    public TextMeshProUGUI text;
    public int id;

    bool selected = false;
    public void InitBtn(int id, string name)
    {
        this.id = id;
        text.text = $"{id}. {name}";
        buttonSelectImg.transform.localScale = new Vector3(1f, 0f, 1f);
    }

    public void OnClick()
    {
        button.interactable = false;
        BookManager.instance.SelectChapter(id + 1);
        buttonSelectImg.LeanScaleY(1f, .7f).setEaseOutExpo();
        ChaptersManagerUI.instance.DeselectOthers(id);
    }

    public void OnDeselect()
    {
        button.interactable = false;
        buttonSelectImg.LeanScaleY(0f, .7f).setEaseOutExpo();
    }
}
