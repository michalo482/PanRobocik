using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCarMission", menuName = "Missions/CarMission")]
public class MissionCarDelivery : Mission
{

    private bool carWasDelivered;
    public override void StartMission()
    {
        FindObjectOfType<MissionObjectCarDeliveryZone>(true).gameObject.SetActive(true);

        string missionText = "Znajdz dzialajacy pojazd.";
        string missionDetails = "Doprowadz go do punktu ewakuacji.";

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);

        carWasDelivered = false;
        MissionObjectCarToDeliver.OnCarDelivery += CarDeliveryCompleted;

        Car[] cars = FindObjectsOfType<Car>();

        foreach (Car car in cars)
        {
            car.AddComponent<MissionObjectCarToDeliver>();
        }
    }

    public override bool MissionCompleted()
    {
        return carWasDelivered;
    }

    private void CarDeliveryCompleted()
    {
        carWasDelivered = true;
        MissionObjectCarToDeliver.OnCarDelivery -= CarDeliveryCompleted;

        UI.instance.inGameUI.UpdateMissionInfo("Zbieraj dupsko, zadanie wykonane.");
    }

}
