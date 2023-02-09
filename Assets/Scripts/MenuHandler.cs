using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MenuHandler : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] private Image PanelImage;
    [SerializeField] private GameObject MenuButtons;
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject InfoPanel;

    [SerializeField] private TMP_Text startText;
    public static MenuHandler singleton;

    private void Awake()
    {
        singleton = this;       
    }

    #region UIHandling

    private void Start()
    { 
        if (GameState.Instance.GetCurrentState() == GameState.GameStates.Menu
            || GameState.Instance.GetCurrentState() == GameState.GameStates.Pause)
        {
            EnableMenuButtons(true);
        }
        else
        {
            EnableMenuButtons(false);
        }

        slider.value = PlayerPrefs.GetFloat("sfx", 1);
        DisableAllButtons();
        startText.text = GetStartButtonText();
    }

    public void OpenInfo()
    {
        DisableAllButtons();
        InfoPanel.SetActive(true);
    }

    public void OpenCredits()
    {
        DisableAllButtons();
        CreditsPanel.SetActive(true);
    }

    private void DisableAllButtons()
    {
        CreditsPanel.SetActive(false);
        InfoPanel.SetActive(false);
    }

   
    public void SetAudio()
    {
        PlayerPrefs.SetFloat("sfx", slider.value);
    }

    public void EnableMenuButtons(bool enable)
    {
        MenuButtons.SetActive(enable);
        DisableAllButtons();

        if(!enable)
        {
            Color c = PanelImage.color;
            c.a = 0f;
            PanelImage.color = c;
        }
        else
        {
            Color c = PanelImage.color;
            c.a = 1f;
            PanelImage.color = c;
        }
    }

    private string GetStartButtonText()
    {
        switch (GameState.Instance.GetCurrentGameState())
        {
            case GameState.GameStates.Menu:
                return "Start";
            case GameState.GameStates.Pause:
                return "Restart";
            default:
                return "Start";
        }
    }
    public float GetSoundVolume()
    {
        float sound = PlayerPrefs.GetFloat("sfx", 1);
        return sound;
    }

    #endregion

    #region ButtonFunctions

    public void StartGame()
    {
        GameState.Instance.TryChangeState(GameState.GameStates.Game);
    }

    public void StartMenu()
    {
        GameState.Instance.TryChangeState(GameState.GameStates.Menu);
    }

    public void StartPause()
    {
        GameState.Instance.TryChangeState(GameState.GameStates.Pause);
    }

    #endregion

}
