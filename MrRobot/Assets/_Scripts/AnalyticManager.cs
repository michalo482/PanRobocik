using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticManager : MonoBehaviour
{
    public static AnalyticManager instance;
    //private bool _isInitialized = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        //_isInitialized = true;

    }

    public void RestartGame()
    {
        AnalyticsService.Instance.RecordEvent("restart_game");
        AnalyticsService.Instance.Flush();
    }

    public void RestartAfterDeath()
    {
        AnalyticsService.Instance.RecordEvent("restart_game_after_death");
        AnalyticsService.Instance.Flush();
    }

    public void HitsFromEnemy()
    {
        AnalyticsService.Instance.RecordEvent("hits_from_enemy");
        AnalyticsService.Instance.Flush();
    }

    public void HitsToEnemy()
    {
        AnalyticsService.Instance.RecordEvent("hits_to_enemy");
        AnalyticsService.Instance.Flush();
    }

    public void AutoRiflePicked()
    {
        AnalyticsService.Instance.RecordEvent("autorifle_picked");
        AnalyticsService.Instance.Flush();
    }

    public void PistolPicked()
    {
        AnalyticsService.Instance.RecordEvent("pistol_picked");
        AnalyticsService.Instance.Flush();
    }

    public void RiflePicked()
    {
        AnalyticsService.Instance.RecordEvent("rifle_picked");
        AnalyticsService.Instance.Flush();
    }

    public void RevolverPicked()
    {
        AnalyticsService.Instance.RecordEvent("revolver_picked");
        AnalyticsService.Instance.Flush();
    }

    public void ShotgunPicked()
    {
        AnalyticsService.Instance.RecordEvent("shotgun_picked");
        AnalyticsService.Instance.Flush();
    }


}
