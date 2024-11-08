using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Audio/Events/AudioEvent")]
public class AudioEvent : ScriptableObject
{
    public UnityEvent OnPlayAudio;

    private bool isRaising = false;

    public void Raise()
  {
    if (isRaising) return; // Sprawdza, czy jest ju¿ uruchomiony

    isRaising = true;
    OnPlayAudio?.Invoke();
    isRaising = false;
    }

}
