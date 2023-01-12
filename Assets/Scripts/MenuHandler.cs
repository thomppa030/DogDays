using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject CreditsPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject InfoPanel;

    private void Start()
    {
        DisableAllButtons();
    }

    public void OpenOptions()
    {
        DisableAllButtons();
        OptionsPanel.SetActive(true);
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
        OptionsPanel.SetActive(false);
        InfoPanel.SetActive(false);
    }
}
