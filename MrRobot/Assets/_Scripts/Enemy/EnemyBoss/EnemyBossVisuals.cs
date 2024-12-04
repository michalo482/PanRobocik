using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossVisuals : MonoBehaviour
{

    [SerializeField] private ParticleSystem landingZoneFX;
    [SerializeField] private GameObject[] weaponTrails;

    private EnemyBoss enemy;
    [SerializeField] private GameObject[] batteries;
    [SerializeField] private float initialBatteryScaleY = 0.0015f;

    private float dischargeSpeed;
    private float rechargeSpeed;

    private bool isRecharging;

    private void Awake()
    {
        enemy = GetComponent<EnemyBoss>();
        landingZoneFX.transform.parent = null;
        landingZoneFX.Stop();
        ResetBatteries();
    }

    private void Update()
    {
        UpdateBatteriesScale();
    }

    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initialBatteryScaleY / enemy.abilityCooldown;
        dischargeSpeed = initialBatteryScaleY / enemy.flamethrowDuration * 1.6f;

        foreach(GameObject battery in batteries)
        {
            battery.SetActive(true);
        }
    }

    public void DischargeBatteries()
    {
        isRecharging = false;
    }

    private void UpdateBatteriesScale()
    {
        if (batteries.Length <= 0)
        {
            return;
        }

        foreach(GameObject battery in batteries)
        {
            if(battery.activeSelf)
            {
                float scaleChange = (isRecharging ? rechargeSpeed : -dischargeSpeed) * Time.deltaTime;
                float newScaleY = Mathf.Clamp(battery.transform.localScale.y + scaleChange, 0, initialBatteryScaleY);

                battery.transform.localScale = new Vector3(0.0015f, newScaleY, 0.0015f); 

                if(battery.transform.localScale.y <= 0)
                {
                    battery.SetActive(false);
                }
            }
        }
    }

    public void PlaceLandingZone(Vector3 target)
    {
        landingZoneFX.transform.position = target;
        
        landingZoneFX.Clear();

        var mainModule = landingZoneFX.main;
        mainModule.startLifetime = enemy.travelTimeToTarget * 2;

        landingZoneFX.Play();
    }

    public void EnableWeaponTrail(bool active)
    {
        if(weaponTrails.Length <= 0)
        {
            return;
        }

        foreach(var trail in weaponTrails)
        {
            trail.gameObject.SetActive(active);
        }
    }
}
