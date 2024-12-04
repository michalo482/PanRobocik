using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Settings : MonoBehaviour
{
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxSliderText;

    [SerializeField] private Slider bgmSlider;
    [SerializeField] private TextMeshProUGUI bgmSliderText;

    private void Start()
    {
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        bgmSlider.value = savedBGMVolume;
        BgmSliderVolume(savedBGMVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxSlider.value = savedSFXVolume;
        SFXSliderVolume(savedSFXVolume);
    }

    public void SFXSliderVolume(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void BgmSliderVolume(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
        AudioManager.Instance.SetBGMVolume(value);
    }

    public void OnFriendlyFireToggle()
    {
        bool friendlyFire = GameManager.Instance.friendlyFire;
        GameManager.Instance.friendlyFire = !friendlyFire;
    }
}
