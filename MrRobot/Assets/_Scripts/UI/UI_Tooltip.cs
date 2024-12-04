using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip?.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip?.SetActive(false);
    }
}
