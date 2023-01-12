using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
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
                LoadScene(1);
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

                //Implement Option menu
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

                //Implement Logic
                //Option Menu needed
            }
            else if (requestedState == GameStates.Menu)
            {
                Debug.Log("Change current state " + currentState + " to " + requestedState);
                currentState = requestedState;

                //Loading Menu Scene from Pause.
                LoadScene(0);
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
}
