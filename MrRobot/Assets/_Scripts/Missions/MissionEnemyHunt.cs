using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHuntMission", menuName = "Missions/HuntMission")]
public class MissionEnemyHunt : Mission
{
    public int amountToKill = 0;
    public EnemyType enemyType;
    //private List<Enemy> enemyList;

    private int killsToGo;

    public override void StartMission()
    {

        killsToGo = amountToKill;

        UpdateMissionUI();

        MissionObjectHuntTarget.OnTargetKIlled += EliminateTarget;
        //throw new System.NotImplementedException();
        //enemyList = FindObjectsOfType<Enemy>().ToList();
        List<Enemy> validEnemies = new List<Enemy>(FindObjectsOfType<Enemy>());

        //if(enemyType == EnemyType.RANDOM)
        //{
        //    validEnemies = enemyList;
        //}
        //else
        //{
        //    //foreach (Enemy enemy in enemyList)
        //    //{
        //    //    if(enemy.enemyType == enemyType)
        //    //    { 
        //    //        validEnemies.Add(enemy);
        //    //    }
        //    //}
        //}



        for (int i = 0; i < amountToKill; i++)
        {
            if(validEnemies.Count <= 0)
            {
                return;
            }
            int randomIndex = Random.Range(0, validEnemies.Count);
            validEnemies[randomIndex].AddComponent<MissionObjectHuntTarget>();
            validEnemies.RemoveAt(randomIndex);
        }

    }
    public override bool MissionCompleted()
    {
        return killsToGo <= 0;
    }


    private void EliminateTarget()
    {
        killsToGo--;
        UpdateMissionUI();

        if(killsToGo <=0)
        {
            UI.instance.inGameUI.UpdateMissionInfo("zbieraj dupsko");
            MissionObjectHuntTarget.OnTargetKIlled -= EliminateTarget;
        }
    }

    private void UpdateMissionUI()
    {
        string missionText = "Wyeliminuj " + amountToKill + " przeciwnikow";
        string missionDetails = "Do zabicia: " + killsToGo;

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);
    }

}
