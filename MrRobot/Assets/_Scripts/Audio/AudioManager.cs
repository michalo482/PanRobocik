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

        SaveOriginalVolumes(); // Zapisujemy bazowe warto�ci

        // Odczytanie zapisanych warto�ci i ustawienie g�o�no�ci na podstawie preferencji
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
            // Zapisujemy oryginaln� g�o�no��, je�li jeszcze tego nie zrobili�my
            if (!originalVolumes.ContainsKey(source))
            {
                originalVolumes.Add(source, source.volume);
            }
        }
    }

    private void Update()
    {
        // Dodajemy nowe AudioSource do s�ownika, je�li pojawi� si� w trakcie gry
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
                // Je�li �r�d�o jest w s�owniku, ustawiamy jego g�o�no��
                if (originalVolumes.ContainsKey(source))
                {
                    source.volume = originalVolumes[source] * volume; // Skalowanie na podstawie oryginalnej g�o�no�ci
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

            // Je�li �r�d�o jest w s�owniku, ustawiamy jego g�o�no��
            if (originalVolumes.ContainsKey(source))
            {
                source.volume = originalVolumes[source] * volume; // Skalowanie na podstawie oryginalnej g�o�no�ci
            }
            else
            {
                Debug.LogWarning($"AudioSource {source.name} nie zosta� znaleziony w s�owniku.");
            }
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
}
