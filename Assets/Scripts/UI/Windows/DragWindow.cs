using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class DragWindow : MonoBehaviour, IDragHandler, IDeselectHandler, IPointerUpHandler, IPointerDownHandler {

    [SerializeField] private RectTransform dragRectTransform;
    [SerializeField] private Canvas canvas;

    public Vector2 xLimits;
    public Vector2 yLimits;

    public void OnDrag(PointerEventData eventData) {
        dragRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        Vector2 clamped = dragRectTransform.anchoredPosition;
        clamped.x = Mathf.Clamp(clamped.x, xLimits.x + dragRectTransform.rect.width / 2, xLimits.y - dragRectTransform.rect.width / 2);
        clamped.y = Mathf.Clamp(clamped.y, yLimits.x + dragRectTransform.rect.height / 2, yLimits.y - dragRectTransform.rect.height / 2);
        dragRectTransform.anchoredPosition = clamped;
        dragged = true;
        if (selected)
            Deselect();
    }

    public bool isIcon;

    public Image img;
    public TextMeshProUGUI tmp;


    public Color defaultColor = Color.white;
    public Color tint = Color.blue;

    public string fileText = "secret.txt";

    public UnityEvent onDoubleClick;

    bool dragged;
    bool selected;
    float time;
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isIcon) return;

        if (dragged)
        {
            dragged = false;
            return;
        }


        if (!selected)
            SelectUI();

        if (Time.unscaledTime - time < 0.5f && selected)
        {
            Deselect();
            time = 0;
            onDoubleClick?.Invoke();
        }
        else
            time = Time.unscaledTime;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!isIcon) return;

        if (selected)
            Deselect();
    }

    public void SelectUI()
    {
        selected = true;
        img.color = tint;
        tmp.text = "<mark=#398BFF40>" + fileText;
    }

    public void Deselect()
    {
        selected = false;
        img.color = defaultColor;
        tmp.text = fileText;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
