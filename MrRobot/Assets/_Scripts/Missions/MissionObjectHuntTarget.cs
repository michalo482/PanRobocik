using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionObjectHuntTarget : MonoBehaviour
{
    public static event Action OnTargetKIlled;

    public void InvokeOnTargetKilled()
    {
        OnTargetKIlled?.Invoke();
    }
}
