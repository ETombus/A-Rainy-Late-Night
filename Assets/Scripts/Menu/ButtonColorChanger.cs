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
    [SerializeField] bool invertColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().color = selectColor;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        this.GetComponentInChildren<TextMeshProUGUI>().color = idleColor;
    }
}
