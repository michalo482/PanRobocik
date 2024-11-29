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

    //[SerializeField] private Toggle friendlyFireToggle;

    public void SFXSliderVolume(float value)
    {
        sfxSliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void BgmSliderVolume(float value)
    {
        bgmSliderText.text = Mathf.RoundToInt(value * 100) + "%";
    }

    public void OnFriendlyFireToggle()
    {
        bool friendlyFire = GameManager.Instance.friendlyFire;
        GameManager.Instance.friendlyFire = !friendlyFire;
    }

}
