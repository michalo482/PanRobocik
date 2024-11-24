using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TransparentOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Dictionary<Image, Color> originalImageColors = new Dictionary<Image, Color>();
    private Dictionary<TextMeshProUGUI, Color> originalTextColors = new Dictionary<TextMeshProUGUI, Color>();

    private bool hasUIWeaponSlots;
    private PlayerWeaponController playerWeaponController;

    private void Start()
    {
        hasUIWeaponSlots = GetComponentInChildren<UI_WeaponSlot>();
        if (hasUIWeaponSlots)
        {
            playerWeaponController = FindObjectOfType<PlayerWeaponController>();
        }

        foreach(var image in GetComponentsInChildren<Image>(true))
        {
            originalImageColors[image] = image.color;
        }

        foreach (var text in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            originalTextColors[text] = text.color;
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var image in originalImageColors.Keys)
        {
            var color = image.color;
            color.a = 0.15f;
            image.color = color;
        }

        foreach (var text in originalTextColors.Keys)
        {
            var textColor = text.color;
            textColor.a = 0.15f;
            text.color = textColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach( var image in originalImageColors.Keys)
        {
            image.color = originalImageColors[image];
        }
        foreach( var text in originalTextColors.Keys)
        {
            text.color = originalTextColors[text];
        }

        playerWeaponController?.UpdateWeaponUI();
    }
}
