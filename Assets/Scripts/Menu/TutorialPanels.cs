using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TutorialPanels : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum Type { Newspaper, Digital }
    [SerializeField] Type thisType;

    [SerializeField] GameObject digitalPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (thisType == Type.Newspaper && digitalPanel != null) //if  it still doenst show up, make sure DigitalPanels parentobject is active!
            digitalPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if (thisType == Type.Digital)
            digitalPanel.SetActive(false);
    }
}
