using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private Dictionary<AudioSource, float> originalVolumes = new Dictionary<AudioSource, float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SaveOriginalVolumes(); // Zapisujemy bazowe wartoœci

        // Odczytanie zapisanych wartoœci i ustawienie g³oœnoœci na podstawie preferencji
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        SetBGMVolume(savedBGMVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        SetSFXVolume(savedSFXVolume);
    }

    private void SaveOriginalVolumes()
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            // Zapisujemy oryginaln¹ g³oœnoœæ, jeœli jeszcze tego nie zrobiliœmy
            if (!originalVolumes.ContainsKey(source))
            {
                originalVolumes.Add(source, source.volume);
            }
        }
    }

    private void Update()
    {
        // Dodajemy nowe AudioSource do s³ownika, jeœli pojawi¹ siê w trakcie gry
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (!originalVolumes.ContainsKey(source))
            {
                originalVolumes.Add(source, source.volume);
            }
        }
    }

    public void SetBGMVolume(float volume)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (source.CompareTag("BGM") || source.CompareTag("BGM_MENU"))
            {
                // Jeœli Ÿród³o jest w s³owniku, ustawiamy jego g³oœnoœæ
                if (originalVolumes.ContainsKey(source))
                {
                    source.volume = originalVolumes[source] * volume; // Skalowanie na podstawie oryginalnej g³oœnoœci
                }
            }
        }

        PlayerPrefs.SetFloat("BGMVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float volume)
    {
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            if (source.CompareTag("BGM") || source.CompareTag("BGM_MENU"))
                continue; // Pomijamy BGM

            // Jeœli Ÿród³o jest w s³owniku, ustawiamy jego g³oœnoœæ
            if (originalVolumes.ContainsKey(source))
            {
                source.volume = originalVolumes[source] * volume; // Skalowanie na podstawie oryginalnej g³oœnoœci
            }
            else
            {
                Debug.LogWarning($"AudioSource {source.name} nie zosta³ znaleziony w s³owniku.");
            }
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}
