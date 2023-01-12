using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
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
}
