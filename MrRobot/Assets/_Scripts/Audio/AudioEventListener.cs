using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioEventListener : MonoBehaviour
{
    [SerializeField] private AudioEvent audioEvent;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool shouldLoop = false;

    private void OnEnable()
    {
        if (audioEvent != null)
            audioEvent.OnPlayAudio.AddListener(PlayAudio);
    }

    private void OnDisable()
    {
        if (audioEvent != null)
            audioEvent.OnPlayAudio.RemoveListener(PlayAudio);
    }

    private void PlayAudio()
    {
        if (audioSource != null)
        {
            audioSource.loop = shouldLoop;

            if (shouldLoop)
            {
                // U¿yj Play dla zapêtlonego dŸwiêku
                if (!audioSource.isPlaying) // Sprawdza, czy dŸwiêk ju¿ gra
                    audioSource.Play();
            }
            else
            {
                // U¿yj PlayOneShot dla jednorazowego efektu
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}

