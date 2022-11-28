using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] bool invertColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        this.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
        if(invertColor) { this.GetComponentInChildren<TextMeshProUGUI>().color = Color.white; }
    }
}
