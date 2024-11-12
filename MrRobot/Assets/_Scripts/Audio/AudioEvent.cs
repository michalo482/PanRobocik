using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Audio/Events/AudioEvent")]
public class AudioEvent : ScriptableObject
{
    public UnityEvent OnPlayAudio;    // Event uruchamiaj�cy d�wi�k
    public UnityEvent OnStopAudio;    // Event zatrzymuj�cy d�wi�k
    private bool isRaising = false;

    private AudioSource audioSource; // Przechowywanie referencji do AudioSource

    public void Raise()
    {
        if (isRaising) return; // Je�li d�wi�k ju� jest odtwarzany, nie wywo�uj ponownie
        isRaising = true;
        OnPlayAudio?.Invoke();
        isRaising = false;
    }

}
