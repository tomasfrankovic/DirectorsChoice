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
    public void InitBtn(int id, string name)
    {
        this.id = id;
        text.text = $"{id + 1}. {name}";
        buttonSelectImg.transform.localScale = new Vector3(1f, 0f, 1f);
        gameObject.SetActive(true);
        text.color = ChaptersManagerUI.instance.deselectColor;
    }

    public void OnSelect()
    {
        LeanTween.cancel(buttonSelectImg);
        buttonSelectImg.SetActive(true);
        text.color = ChaptersManagerUI.instance.selectedColor;
        button.interactable = false;
        BookManager.instance.SelectChapter(id + 1);
        buttonSelectImg.LeanScaleY(1f, .7f).setEaseOutExpo();
        ChaptersManagerUI.instance.DeselectOthers(id);
    }

    public void OnDeselect()
    {
        LeanTween.cancel(buttonSelectImg);
        text.color = ChaptersManagerUI.instance.deselectColor;
        button.interactable = true;
        buttonSelectImg.LeanScaleY(0f, .35f).setEaseOutExpo().setOnComplete(() => { buttonSelectImg.SetActive(false); });
    }
}
