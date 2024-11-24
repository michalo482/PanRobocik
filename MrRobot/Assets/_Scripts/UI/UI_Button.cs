using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public float scaleSpeed = 3;
    public float scaleRate = 1.2f;

    private Vector3 defaultScale;
    private Vector3 targetScale;

    private Image buttonImage;
    private TextMeshProUGUI buttonText;

    public virtual void Start()
    {
        defaultScale = transform.localScale;
        targetScale = defaultScale;

        buttonImage = GetComponent<Button>().image;
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public virtual void Update()
    {
        if(Mathf.Abs(transform.localScale.x - targetScale.x) > 0.01f)
        {
            float scaleValue = Mathf.Lerp(transform.localScale.x, targetScale.x, Time.deltaTime * scaleSpeed);

            transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = defaultScale * scaleRate;
        buttonImage.color = Color.yellow;
        buttonText.color = Color.yellow;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ReturnToDefaultLook();
    }

    private void ReturnToDefaultLook()
    {
        targetScale = defaultScale;

        buttonImage.color = Color.white;
        buttonText.color = Color.white;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        ReturnToDefaultLook();
    }
}
