using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_InGame inGameUI { get; private set; }
    public UI_WeaponSelection weaponSelection { get; private set; }
    public UI_GameOver gameOver {  get; private set; }
    public GameObject pauseUI;
    public GameObject winScreen;
    [SerializeField] private GameObject[] uiElements;
    [SerializeField] private Image fadeImage;
    

    private void Awake()
    {
        instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
        weaponSelection = GetComponentInChildren<UI_WeaponSelection>(true);
        gameOver = GetComponentInChildren<UI_GameOver>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        AssignInputsUI();
        StartCoroutine(ChangeImageAlpha(0, 1.5f, null));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchTo(GameObject uiToSwitchOn)
    {
        foreach (var go in uiElements)
        {
            go.SetActive(false);
        }

        uiToSwitchOn.SetActive(true);
    }

    public void StartTheGame()
    {

        StartCoroutine(StartGameSecuence());
        //SwitchTo(inGameUI.gameObject);
        //GameManager.Instance.GameStart();
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void RestartTheGame()
    {
        AnalyticManager.instance.RestartGame();
        StartCoroutine(ChangeImageAlpha(1, 1f, GameManager.Instance.RestartScene));      
    }

    public void PauseSwitch()
    {
        bool gamePaused = pauseUI.activeSelf;

        if(gamePaused)
        {
            SwitchTo(inGameUI.gameObject);
            ControlsManager.Instance.SwitchToCharacterControls();
            TimeManager.instance.ResumeTime();
        }
        else
        {
            SwitchTo(pauseUI);
            ControlsManager.Instance.SwitchToUIControls();
            TimeManager.instance.PauseTime();
        }
    }

    private void AssignInputsUI()
    {
        PlayerControlls controls = GameManager.Instance.player.Controls;

        controls.UI.UIPause.performed += ctx => PauseSwitch();
    }

    public void ShowGameOverUI(string message = "GameOver!")
    {
        AnalyticManager.instance.RestartAfterDeath();
        SwitchTo(gameOver.gameObject);
        gameOver.ShowGameOverMessage(message);
    }

    private IEnumerator ChangeImageAlpha(float targetAlpha, float duration, System.Action onComplete)
    {
        float time = 0;

        Color currentColor = fadeImage.color;
        float startingAlpha = currentColor.a;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startingAlpha, targetAlpha, time / duration);

            fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        fadeImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        onComplete?.Invoke();
    }

    private IEnumerator StartGameSecuence()
    {
        StartCoroutine(ChangeImageAlpha(1, 1, null));

        yield return new WaitForSeconds(1);
        SwitchTo(inGameUI.gameObject);
        GameManager.Instance.GameStart();
        StartCoroutine(ChangeImageAlpha(0, 1, null));
    }

    public void StartLevelGeneration() => LevelGenerator.instance.InitializeGeneration();

    public void ShowWinScreen()
    {
        StartCoroutine(ChangeImageAlpha(1, 1.5f, SwitchToWinScreen));
    }

    private void SwitchToWinScreen()
    {
        SwitchTo(winScreen);

        Color color = fadeImage.color;
        color.a = 0;
        fadeImage.color = color;
    }
}
