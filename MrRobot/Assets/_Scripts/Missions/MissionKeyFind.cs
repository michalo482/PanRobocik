using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyMission", menuName = "Missions/KeyMission")]
public class MissionKeyFind : Mission
{
    [SerializeField] private GameObject key;
    //[SerializeField] private GameObject enemyPrefab;
    private bool keyFound;

    public override void StartMission()
    {
        MissionObjectKey.OnKeyPickUp += PickUpKey;

        UI.instance.inGameUI.UpdateMissionInfo("Znajdz dupka z kluczem");

        List<Enemy> enemiesOnLevel = new List<Enemy>(FindObjectsOfType<Enemy>());
        Enemy randomEnemy = enemiesOnLevel[Random.Range(0, enemiesOnLevel.Count)];
        randomEnemy.GetComponent<EnemyDropController>()?.GiveKey(key);
        randomEnemy.MakeEnemyVIP();
        
    }
    public override bool MissionCompleted()
    {
        return keyFound;
    }

    private void PickUpKey()
    {
        keyFound = true;
        MissionObjectKey.OnKeyPickUp -= PickUpKey;
        UI.instance.inGameUI.UpdateMissionInfo("Masz klucz! \n Get to da chopper!");
    }

}
