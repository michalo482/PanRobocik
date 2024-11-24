using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SelectMission : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI missionDescription;

    public void UpdateMissionDescription(string text)
    {
        missionDescription.text = text;
    }
}
