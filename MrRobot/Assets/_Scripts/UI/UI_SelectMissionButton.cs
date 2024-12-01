using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SelectMissionButton : UI_Button
{
    private UI_SelectMission missionUI;
    [SerializeField] private Mission mission;
    private TextMeshProUGUI missionText;

    private void OnValidate()
    {
        gameObject.name = "ButtonSelectMission" + mission.missionName;
        //missionText.text = mission.missionName;
    }
    public override void Start()
    {
        base.Start();

        missionUI = GetComponentInParent<UI_SelectMission>(true);
        missionText = GetComponentInChildren<TextMeshProUGUI>();
        missionText.text = mission.missionName;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        missionUI.UpdateMissionDescription(mission.missionDescription);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        MissionManager.instance.SetMission(mission);
        //UI.instance.SwitchToInGameUI();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        missionUI.UpdateMissionDescription("Choose a mission");
    }
}
