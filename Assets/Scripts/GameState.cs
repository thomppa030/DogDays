using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private int menuID = 0;
    [SerializeField] private int gameID = 1;

    public enum GameStates
    {
        Menu,
        Game,
        Pause
    }
    public GameStates currentState = GameStates.Menu;
    public static GameState Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("Instance is null - using this object:" + gameObject.name);
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameState initialised. Current State: " + currentState);
        }
        else
        {
            Debug.LogWarning("Instance is already filled and not null - sending this object home: " + gameObject.name);
            GameObject.Destroy(gameObject);
        }
    }

    private void Start()
    {
        TryChangeState(currentState);
    }

    public void TryChangeState(GameStates requestedState)
    {
        Debug.Log("Trying to change Gamestate from " + currentState + " to " + requestedState);

        #region Current State: Menu
        if (currentState == GameStates.Menu)
        {
            if (requestedState == GameStates.Game)
            {
                Debug.Log("Change current state " + currentState + " to " + requestedState);
                currentState = requestedState;

                //Loading Game Scene from Menu.
                LoadScene(gameID);
            }
        }
        #endregion
        #region Current State: Ingame
        else if (currentState == GameStates.Game)
        {
            if (requestedState == GameStates.Pause)
            {
                Debug.Log("Change current state " + currentState + " to " + requestedState);
                currentState = requestedState;

                MenuHandler.singleton.SetStartButtonText();
                MenuHandler.singleton.EnableMenuButtons(true);
                MenuHandler.singleton.GetMainMenuButton();
            }
            
        }
        #endregion
        #region Current State: Pause
        else if (currentState == GameStates.Pause)
        {
            if (requestedState == GameStates.Game)
            {
                Debug.Log("Change current state " + currentState + " to " + requestedState);
                currentState = requestedState;

                MenuHandler.singleton.EnableMenuButtons(false);               
            }
            else if (requestedState == GameStates.Menu)
            {
                Debug.Log("Change current state " + currentState + " to " + requestedState);
                currentState = requestedState;

                //Loading Menu Scene from Pause.
                LoadScene(menuID);
            }
        }
        #endregion
    }

    public GameStates GetCurrentState()
    {
        return currentState;
    }

    private void LoadScene(int sceneID)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneID);
    }

    public GameStates GetCurrentGameState()
    {
        return currentState;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (currentState)
            {
                case GameStates.Game:
                    TryChangeState(GameStates.Pause);
                    break;
                case GameStates.Pause:
                    TryChangeState(GameStates.Game);
                    break;
                default:
                    break;
            }
        }
    }
}
