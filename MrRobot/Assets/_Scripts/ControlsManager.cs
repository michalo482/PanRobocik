using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance;
    public PlayerControlls Controls {  get; private set; }
    private Player player;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Controls = GameManager.Instance.player.Controls;
        //Controls = new PlayerControlls();
        player = GameManager.Instance.player;

        SwitchToCharacterControls();
    }

    public void SwitchToCharacterControls()
    {
        Controls.UI.Disable();
        Controls.Character.Enable();
        player.SetControlsEnabledTo(true);
    }

    public void SwitchToUIControls()
    {
        Controls.UI.Enable();
        Controls.Character.Disable();
        player.SetControlsEnabledTo(false);
    }
}
