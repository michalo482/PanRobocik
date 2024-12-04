using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LastDefenceMission", menuName = "Missions/LastDefenceMission")]
public class MissionLastDefence : Mission
{
    private bool defenceBegun = false;

    [Header("Cooldown & duration")]
    public float defenceDuration = 120;
    private float defenceTimer;
    public float waveCooldown = 15;
    private float waveTimer;


    [Header("Respawn details")]
    public int amountOfRespawnPoints = 2;
    public List<Transform> respawnPoints;
    private Vector3 defencePoint;
    [Space]
    public int enemiesPerWave;
    public GameObject[] posibleEnemies;

    private string defenceTimerText;

    private void OnEnable()
    {
        defenceBegun = false;
    }

    public override void StartMission()
    {
        defencePoint = FindObjectOfType<MissionEndTrigger>().transform.position;
        respawnPoints = new List<Transform>(ClosestPoints(amountOfRespawnPoints));

        UI.instance.inGameUI.UpdateMissionInfo("Zbieraj sie do punktu ewakuacji.");
    }

    public override void UpdateMission()
    {
        if(defenceBegun == false)
        {
            return;
        }

        waveTimer -= Time.deltaTime;
        if(defenceTimer > 0)
        {
            defenceTimer -= Time.deltaTime;
        }

        if(waveTimer < 0)
        {
            CreateNewEnemies(enemiesPerWave);
            waveTimer = waveCooldown;
        }

        defenceTimerText = System.TimeSpan.FromSeconds(defenceTimer).ToString("mm':'ss");

        string missionText = "Bron samolotu poki nie bedzie gotowy do odlotu";
        string missionDetails = "wytrzymaj jeszcze + " + defenceTimerText;

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);

    }
    public override bool MissionCompleted()
    {
        if(defenceBegun == false)
        {
            StartDefenceEvent();
            return false;
        }

        return defenceTimer < 0;
    }


    private List<Transform> ClosestPoints(int amount)
    {
        List<Transform> closestPoints = new List<Transform>();
        List<MissionObjectEnemyRespawnPoint> allPoints = new List<MissionObjectEnemyRespawnPoint>(FindObjectsOfType<MissionObjectEnemyRespawnPoint>());

        while (closestPoints.Count < amount && allPoints.Count > 0)
        {
            float shortestDistance = float.MaxValue;
            MissionObjectEnemyRespawnPoint closestPoint = null;

            foreach (var point in allPoints)
            {
                float distance = Vector3.Distance(point.transform.position, defencePoint);
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = point;
                }
            }

            if (closestPoint != null)
            {
                closestPoints.Add(closestPoint.transform);
                allPoints.Remove(closestPoint);
            }

        }

        return closestPoints;
    }

    private void CreateNewEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform randomRespawnPoint = respawnPoints[Random.Range(0, respawnPoints.Count)];

            int randomEnemyIndex = Random.Range(0, posibleEnemies.Length);
            GameObject randomEnemy = posibleEnemies[randomEnemyIndex];

            randomEnemy.GetComponent<Enemy>().aggressionRange = 100;

            ObjectPool.Instance.GetObject(randomEnemy, randomRespawnPoint);
        }

    }

    private void StartDefenceEvent()
    {
        waveTimer = 0.5f;
        defenceTimer = defenceDuration;
        defenceBegun = true;
    }
}
