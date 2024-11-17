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

    // Opcjonalnie: metoda do wywo³ania rêcznego
    public void TriggerAudioEvent()
    {
        if (audioEvent != null)
        {
            audioEvent.Raise();
        }
    }
}
