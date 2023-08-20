using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    MENU = 0,
    PLAYING = 1,
    SETTING = 2,
    LEVELPASS = 3,
    WIN = 4,
    MAX_STATE = 5
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] LevelManager levelManager = null;
    [SerializeField] UIManager uiManager = null;
    private GameState gameState = GameState.MENU;

    void Start()
    {
        // loading managers
        if(levelManager == null)
            levelManager = FindObjectOfType<LevelManager>();
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();

        OnInit();
    }

    public void OnInit()
    {
        ResetAll();
        SetGameState(GameState.MENU);
    }

    private void ResetAll()
    {
        levelManager.OnInit();
        uiManager.OnInit();
    }

    public void StartNewGame()
    {
        SetGameState(GameState.PLAYING);
    }

    /// <summary>
    /// Resume game after setting
    /// </summary>
    public void Continue()
    {
        SetGameState(GameState.PLAYING);
    }
    /// <summary>
    /// Open Setting canvas
    /// </summary>
    public void Setting()
    {
        SetGameState(GameState.SETTING);
    }

    public void NextLevel()
    {
        levelManager.NextLevel();
        SetGameState(GameState.PLAYING);
    }

    public void RePlay()
    {
        levelManager.RePlay();
        SetGameState(GameState.PLAYING);
    }

    public void SetGameState(GameState state)
    {
        gameState = state;

        if (state == GameState.PLAYING)
            levelManager.Resume();
        else
            levelManager.Pause();

        uiManager.SetGameState(state);
    }
}
