using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TMP_Text))]
public class LinkClick : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI m_textMeshPro;
    private Camera cam;
    void Start()
    {
        m_textMeshPro = GetComponent<TextMeshProUGUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (cam == null)
            cam = Camera.main;

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = m_textMeshPro.textInfo.linkInfo[linkIndex];
            //Debug.Log($"Link ID is: {linkInfo.GetLinkID()} //and position is: {CalcLinkCenterPosition(linkIndex)}");
            SpellingCorrectionUI.instance.InitSpellingCorection(CalcLinkCenterPosition(linkIndex));
        }
    }

    Vector2 CalcLinkCenterPosition(int linkIndex)
    {
        Transform m_Transform = gameObject.GetComponent<Transform>();

        Vector3 bottomLeft = Vector3.zero;
        Vector3 topRight = Vector3.zero;

        float maxAscender = -Mathf.Infinity;
        float minDescender = Mathf.Infinity;

        TMP_TextInfo textInfo = m_textMeshPro.textInfo;
        TMP_LinkInfo linkInfo = textInfo.linkInfo[linkIndex];

        //int linkMiddleCharIndex = linkInfo.linkTextfirstCharacterIndex + linkInfo.linkTextLength / 2; //middle of the word
        int linkMiddleCharIndex = linkInfo.linkTextfirstCharacterIndex;
        TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[linkMiddleCharIndex];

        maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
        minDescender = Mathf.Min(minDescender, currentCharInfo.descender);

        bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);

        bottomLeft = m_Transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
        topRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));

        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;

        Vector2 centerPosition = bottomLeft;
        centerPosition.x += width / 2;
        centerPosition.y += height / 2;

        return centerPosition;
    }
}