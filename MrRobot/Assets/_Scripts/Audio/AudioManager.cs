using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource deathSource;
    public AudioSource healSource;
    public AudioSource healSource2;
    public AudioSource damageSource;
    public AudioSource deadSource;
    public AudioSource repairSource;

    [Header("Playlists")]
    public List<AudioClip> menuPlaylist;  
    public List<AudioClip> gamePlaylist;
    private AudioSource audioSource;
    private List<AudioClip> currentPlaylist;
    private int currentTrackIndex = 0; 

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

        audioSource = GetComponent<AudioSource>();
        SaveOriginalVolumes(); // Zapisujemy bazowe warto�ci

        // Odczytanie zapisanych warto�ci i ustawienie g�o�no�ci na podstawie preferencji
        float savedBGMVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);
        SetBGMVolume(savedBGMVolume);

        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        SetSFXVolume(savedSFXVolume);
    }


    public void PlayMenuMusic()
    {
        if (currentPlaylist == menuPlaylist) return; 

        StopMusic();  
        currentPlaylist = new List<AudioClip>(menuPlaylist); 
        ShufflePlaylist();
        PlayNextTrack();
    }

    public void PlayGameMusic()
    {
        if (currentPlaylist == gamePlaylist) return; 

        StopMusic();
        currentPlaylist = new List<AudioClip>(gamePlaylist);
        ShufflePlaylist();
        PlayNextTrack();
    }

    void PlayNextTrack()
    {
        if (currentPlaylist == null || currentPlaylist.Count == 0) return;

        audioSource.clip = currentPlaylist[currentTrackIndex];
        audioSource.Play();

        currentTrackIndex = (currentTrackIndex + 1) % currentPlaylist.Count; 

        
        Invoke(nameof(PlayNextTrack), audioSource.clip.length);
    }

    void ShufflePlaylist()
    {
        for (int i = 0; i < currentPlaylist.Count; i++)
        {
            int randomIndex = Random.Range(i, currentPlaylist.Count);
            AudioClip temp = currentPlaylist[i];
            currentPlaylist[i] = currentPlaylist[randomIndex];
            currentPlaylist[randomIndex] = temp;
        }
        currentTrackIndex = 0;
    }

    public void StopMusic()
    {
        audioSource.Stop();
        CancelInvoke(nameof(PlayNextTrack));  
    }

    public void PlayHealthSound(string soundType, bool loop = false)
    {
        AudioSource sourceToPlay = null;

        switch (soundType)
        {
            case "death":
                sourceToPlay = deathSource;
                break;
            case "heal":
                sourceToPlay = healSource;
                break;
            case "heal2":
                sourceToPlay = healSource2;
                break;
            case "damage":
                sourceToPlay = damageSource;
                break;
            case "dead":
                sourceToPlay = deadSource;
                break;
            case "repair":
                sourceToPlay = repairSource;
                break;
        }

        if (sourceToPlay != null && !sourceToPlay.isPlaying)
        {
            sourceToPlay.Play();
        }
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
                // Rejestrujemy nowe �r�d�o d�wi�ku z uwzgl�dnieniem ustawionych preferencji g�o�no�ci
                source.volume *= PlayerPrefs.GetFloat("SFXVolume", 1.0f);
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
        // Ustawiamy g�o�no�� dla ka�dego AudioSource, nie tylko tych, kt�re s� nowe
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource source in allAudioSources)
        {
            // Pomijamy BGM, tylko zmieniamy SFX
            if (source.CompareTag("BGM") || source.CompareTag("BGM_MENU"))
                continue;

            // Je�li �r�d�o jest w s�owniku, ustawiamy jego g�o�no��
            if (originalVolumes.ContainsKey(source))
            {
                source.volume = originalVolumes[source] * volume; // Skalowanie na podstawie oryginalnej g�o�no�ci
            }
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }
    
    public void PlaySFX(AudioClip clip, float volumeScale = 1.0f)
    {
        if (clip == null) return;
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();  // Tymczasowy AudioSource
        sfxSource.clip = clip;
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume", 1.0f) * volumeScale;  // Ustawiona globalna g�o�no�� SFX
        sfxSource.Play();
    }       

    public void RefreshSFXVolume()
    {
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1.0f));  // Odczyt z ustawie� i ponowne zastosowanie
    }

    public void RegisterNewAudioSource(AudioSource newSource)
    {
        if (!originalVolumes.ContainsKey(newSource))
        {
            originalVolumes.Add(newSource, newSource.volume);
            newSource.volume *= PlayerPrefs.GetFloat("SFXVolume", 1.0f); // Ustawiamy aktualn� g�o�no�� SFX
        }
    }   
}
