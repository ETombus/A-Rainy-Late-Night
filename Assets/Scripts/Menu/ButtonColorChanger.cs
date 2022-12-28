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
    TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        idleColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = selectColor;
        if (this.transform.childCount > 1)
            this.transform.GetChild(1).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = idleColor;
        if (this.transform.childCount > 1)
            this.transform.GetChild(1).gameObject.SetActive(false);
    }
}
