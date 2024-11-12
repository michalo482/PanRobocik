using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Audio/Events/AudioEvent")]
public class AudioEvent : ScriptableObject
{
    public UnityEvent OnPlayAudio;    // Event uruchamiaj¹cy dŸwiêk
    public UnityEvent OnStopAudio;    // Event zatrzymuj¹cy dŸwiêk
    private bool isRaising = false;

    private AudioSource audioSource; // Przechowywanie referencji do AudioSource

    public void Raise()
    {
        if (isRaising) return; // Jeœli dŸwiêk ju¿ jest odtwarzany, nie wywo³uj ponownie
        isRaising = true;
        OnPlayAudio?.Invoke();
        isRaising = false;
    }

}
