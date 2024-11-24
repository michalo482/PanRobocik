using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI instance;

    public UI_InGame inGameUI { get; private set; }
    [SerializeField] private GameObject[] uiElements;
    

    private void Awake()
    {
        instance = this;
        inGameUI = GetComponentInChildren<UI_InGame>(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
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

    public void SwitchToInGameUI()
    {
        SwitchTo(inGameUI.gameObject);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }
}
