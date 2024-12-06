using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomBGMMenu : MonoBehaviour
{
    public List<AudioSource> bgmSources; 
    private List<AudioSource> trackQueue;
    private AudioSource currentTrack;

    void Start()
    {
        PlayNextTrack();
        InitializeQueue();
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

    public void PlayMusic()
    {
        if (currentTrack != null && currentTrack.isPlaying)
            return; // Je�li co� ju� gra, nie r�b nic

        PlayNextTrack();
    }

    public void StopMusic()
    {
        if (currentTrack != null)
        {
            currentTrack.Stop();
            CancelInvoke(nameof(PlayNextTrack)); // Anuluj zaplanowane odtworzenie
        }
    }

    void PlayNextTrack()
    {
        if (trackQueue.Count == 0)
        {
            InitializeQueue(); // Restart kolejki po odtworzeniu wszystkich utwor�w
        }

        currentTrack = trackQueue[0];
        trackQueue.RemoveAt(0);
        currentTrack.Play();
        Invoke(nameof(PlayNextTrack), currentTrack.clip.length); // Odtw�rz nast�pny po zako�czeniu bie��cego
    }
}
