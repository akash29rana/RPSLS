using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameStates
	{
        None,
		MainMenu,
        Playing,
        Pause,
        Result,
        GameOver,

	}
    // Start is called before the first frame update
    private static GameStateManager _instance;

    public static GameStateManager gameStateManagerInstance
    {
        get
        {
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    private void Awake()
    {
        currentGameState = GameStates.None;
        previousGameState = GameStates.None;
        // If an instance already exists and it's not this one, destroy this object
        if (_instance == null)
        {
            _instance = this;
        }
    }

    GameStates currentGameState;
    GameStates previousGameState;
    public GameStates getCurrentGameState()
    {
        return currentGameState;
    }
    public void setCurrentGameState(GameStates state)
    {
        setPreviousGameState(currentGameState);
        currentGameState = state;
    }
    public GameStates getPreviousGameState()
    {
        return previousGameState;
    }
    public void setPreviousGameState(GameStates state)
    {
        previousGameState = state;
    }


}
