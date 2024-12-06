using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_WeaponSelection : MonoBehaviour
{
    
    [SerializeField] private GameObject nextUIToSwitchOn;
    public UI_SelectedWeaponWindow[] selectedWeapon;

    [Header("Warning Info")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float disaperaingSpeed = .25f;
    private float currentWarningAlpha;
    private float targetWarningAlpha;


    private void Start()
    {
        selectedWeapon = GetComponentsInChildren<UI_SelectedWeaponWindow>();
    }

    private void Update()
    {
        if (currentWarningAlpha > targetWarningAlpha)
        {
            currentWarningAlpha -= Time.deltaTime * disaperaingSpeed;
            warningText.color = new Color(1, 1, 1, currentWarningAlpha);
        }
    }

    public void ConfirmWeaponSelection()
    {
        if(AtLeastOneWeaponSelected())
        {
            foreach(var weapon in SelectedWeaponData())
            {
                if(weapon.weaponType == WeaponType.Pistol)
                {
                    AnalyticManager.instance.PistolPicked();
                }
                if(weapon.weaponType == WeaponType.AutoRifle)
                {
                    AnalyticManager.instance.AutoRiflePicked();
                }
                if (weapon.weaponType == WeaponType.Shotgun)
                {
                    AnalyticManager.instance.ShotgunPicked();
                }
                if (weapon.weaponType == WeaponType.Revolver)
                {
                    AnalyticManager.instance.RevolverPicked();
                }
                if (weapon.weaponType == WeaponType.Rifle)
                {
                    AnalyticManager.instance.RiflePicked();
                }
            }

            GameManager.Instance.SetDefaultWeaponsForPlayer();
            UI.instance.StartLevelGeneration();
            UI.instance.SwitchTo(nextUIToSwitchOn);
            ControlsManager.Instance.SwitchToCharacterControls();
        }
        else
        {
            ShowWarningMessage("Wybierz przynajmniej jedna bron");
        }
    }

   
    private bool AtLeastOneWeaponSelected() => SelectedWeaponData().Count > 0;

    public List<WeaponData> SelectedWeaponData()
    {
        List<WeaponData> selectedData = new List<WeaponData>();

        foreach (UI_SelectedWeaponWindow weapon in selectedWeapon)
        {
            if (weapon.weaponData != null)
                selectedData.Add(weapon.weaponData);
        }

        return selectedData;
    }

    public UI_SelectedWeaponWindow FindEmptySlot()
    {
        for (int i = 0; i < selectedWeapon.Length; i++)
        {
            if (selectedWeapon[i].IsEmpty())
                return selectedWeapon[i];
        }

        return null;
    }
    public UI_SelectedWeaponWindow FindSlowWithWeaponOfType(WeaponData weaponData)
    {
        for (int i = 0; i < selectedWeapon.Length; i++)
        {
            if (selectedWeapon[i].weaponData == weaponData)
                return selectedWeapon[i];
        }

        return null;
    }

    public void ShowWarningMessage(string message)
    {
        warningText.color = Color.white;
        warningText.text = message;

        currentWarningAlpha = warningText.color.a;
        targetWarningAlpha = 0;
    }
}
