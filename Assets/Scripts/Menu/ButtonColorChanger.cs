using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color idleColor;
    [SerializeField] Color selectColor;
    enum TextType { TMPro, Text, NoText }
    [SerializeField] TextType thisTextType;

    TextMeshProUGUI tmpProText;
    Text unityText;

    private void Start()
    {
        switch (thisTextType)
        {
            case TextType.TMPro:
                tmpProText = GetComponentInChildren<TextMeshProUGUI>();
                idleColor = tmpProText.color;
                break;
            case TextType.Text:
                unityText = GetComponentInChildren<Text>();
                idleColor = unityText.color;
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        switch (thisTextType)
        {
            case TextType.TMPro:
                tmpProText.color = selectColor;
                break;
            case TextType.Text:
                unityText.color = selectColor;
                break;
            default:
                break;
        }
        if (this.transform.childCount > 1)
            this.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetColor();
    }

    public void ResetColor()
    {
        switch (thisTextType)
        {
            case TextType.TMPro:
                tmpProText.color = idleColor;
                break;
            case TextType.Text:
                unityText.color = idleColor;
                break;
            default:
                break;
        }
        if (this.transform.childCount > 1)
            this.transform.GetChild(1).gameObject.SetActive(false);
    }
}
