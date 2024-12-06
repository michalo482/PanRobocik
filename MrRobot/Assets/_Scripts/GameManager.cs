using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;

    [Header("Settings")]
    public bool friendlyFire;

    private void Awake()
    {
        Instance = this;

        player = FindObjectOfType<Player>();
    }


    public void SetDefaultWeaponsForPlayer()
    {
        List<WeaponData> newList = UI.instance.weaponSelection.SelectedWeaponData();
        player.Weapon.SetDefaultWeapon(newList);
    }

    public void GameStart()
    {
        SetDefaultWeaponsForPlayer();
        //LevelGenerator.instance.InitializeGeneration();

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GameOver()
    {
        TimeManager.instance.SlowMotionFor(2);
        UI.instance.ShowGameOverUI();
        CameraManager.instance.ChangeCameraDistance(5);
    }

    public void GameCompleted()
    {
        UI.instance.ShowWinScreen();
        ControlsManager.Instance.Controls.Character.Disable();
        player.PlayerHealth.currentHealth += 9999999;
    }
}
