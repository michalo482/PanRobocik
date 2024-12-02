using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBGMPlayer : MonoBehaviour
{
    public List<AudioSource> bgmSources; // Przypisz AudioSource'y w edytorze Unity
    private List<AudioSource> trackQueue; // Kolejka do odtwarzania
    [SerializeField] private AudioSource missionsfx;

    void Start()
    {
        InitializeQueue();
        PlayNextTrack();
        missionsfx.Play();
    }

    void InitializeQueue()
    {
        trackQueue = new List<AudioSource>(bgmSources);
        ShuffleQueue();
    }

    void ShuffleQueue()
    {
        for (int i = 0; i < trackQueue.Count; i++)
        {
            int randomIndex = Random.Range(i, trackQueue.Count);
            AudioSource temp = trackQueue[i];
            trackQueue[i] = trackQueue[randomIndex];
            trackQueue[randomIndex] = temp;
        }
    }

    void PlayNextTrack()
    {
        if (trackQueue.Count == 0)
        {
            InitializeQueue(); // Restart kolejki po odtworzeniu wszystkich utworów
        }

        AudioSource currentTrack = trackQueue[0];
        trackQueue.RemoveAt(0);
        currentTrack.Play();
        Invoke(nameof(PlayNextTrack), currentTrack.clip.length); // Odtwórz nastêpny po zakoñczeniu bie¿¹cego
    }
}
