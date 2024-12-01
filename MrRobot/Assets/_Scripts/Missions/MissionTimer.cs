using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTimerMission", menuName = "Missions/TimerMission")]
public class MissionTimer : Mission
{

    public float time;

    private float currentTime;
    public override void StartMission()
    {
        currentTime = time;
    }

    public override void UpdateMission()
    {
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            GameManager.Instance.GameOver();
        }

        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");

        string missionText = "Zbieraj sie do punktu ewakuacji zanim wszystko wybuchnie";
        string missionDetails = "Do wybuchu " + timeText;

        UI.instance.inGameUI.UpdateMissionInfo(missionText, missionDetails);

        //Debug.Log(timeText);
    }
    public override bool MissionCompleted()
    {
        return currentTime > 0;
    }

}
