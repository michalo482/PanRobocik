using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BetweenPanelUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private Image[] images;
    [SerializeField] private int imageIndex;
    [SerializeField] private TextMeshProUGUI[] textsOfImage;    
    [SerializeField] private GameObject playButton;

    private Image mainImage;
    private bool showIsOver;

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowNextImageOnClick();
    }

    private void Start()
    {
        mainImage = GetComponent<Image>();
        ShowNextImage();
    }

    private void ShowNextImage()
    {
        if(showIsOver)
        {
            return;
        }
        StartCoroutine(ChangeImageAlpha(1, 1.5f, ShowNextImage));
    }

    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;

        Color currentColor = images[imageIndex].color;        
        float startingAlpha = currentColor.a;
        

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startingAlpha, targetAlpha, time / duration);

            images[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);           
            textsOfImage[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        images[imageIndex].color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        imageIndex++;

        if(imageIndex >= images.Length)
        {
            EnablePlayButton();
        }

        onComplete?.Invoke();
    }

    private void EnablePlayButton()
    {
        StopAllCoroutines();
        showIsOver = true;
        playButton.SetActive(true);
        mainImage.raycastTarget = false;
    }

    private void ShowNextImageOnClick()
    {
        if(imageIndex >= images.Length)
        {
            EnablePlayButton();
        }

        images[imageIndex].color = Color.white;
        textsOfImage[imageIndex].color = Color.white;
        imageIndex++;


        if (showIsOver) 
        {
            return;
        }

        ShowNextImage();
    }
}
