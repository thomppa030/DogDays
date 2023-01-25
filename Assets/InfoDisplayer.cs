using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplayer : MonoBehaviour
{
    TMP_Text description;
    float timer;
    bool isActive;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        description = GetComponent<TMP_Text>();
    }

    public void ShowInfo(string text, float showTime)
    {
        description.text = text;
        timer = showTime;
        isActive = true;
        anim.Play("EnableInfo");
    }

    private void Update()
    {
        if(GameState.Instance.GetCurrentGameState() == GameState.GameStates.Game 
            && isActive)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                isActive = false;
                DisableInfo();
            }
        }
    }

    public void DisableInfo()
    {
        anim.Play("DisableInfo");
    }
}
