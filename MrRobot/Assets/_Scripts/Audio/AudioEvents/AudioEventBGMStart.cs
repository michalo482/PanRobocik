using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AudioEventBGMStart : MonoBehaviour
{
    [SerializeField] private AudioEvent audioEvent; // Przypisz tutaj AudioEvent w inspektorze

    private void Start()
    {
        if (audioEvent != null)
        {
            audioEvent.Raise();
        }
    }

    // Opcjonalnie: metoda do wywo�ania r�cznego
    public void TriggerAudioEvent()
    {
        if (audioEvent != null)
        {
            audioEvent.Raise();
        }
    }
}
