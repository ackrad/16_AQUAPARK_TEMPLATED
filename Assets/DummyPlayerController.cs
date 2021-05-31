using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// This script is written to show how this template works within gameplay logic.
/// Try functions with buttons and see how game is started or completed or levels are loaded with this template 
/// </summary>
public class DummyPlayerController : MonoBehaviour
{
    private GameController _gameController;
    private LevelManager _levelManager;

    void Start()
    {
        _gameController = GameController.request();
        _levelManager = LevelManager.request();
    }

    [Button("Lost Game")]
    public void LostGame()
    {
        _gameController.LostGame();
    }

    [Button("Win Game")]
    public void WinGame()
    {
        _gameController.WinGame();

        if (_levelManager.Mode == LevelManager.ManagerOptions.SingleScene)
            _gameController.EarnMoney(LevelManager.request().ActiveLevelData.Price);
        
    }
    
    /// <summary>
    /// This is function logical if auto start at game controller is not checked and player have to start game by manually
    /// </summary>
    [Button("Start Game")]
    public void StartGame()
    {
        _gameController.StartGame();   
    }
    
    /// <summary>
    /// This is function logical if auto load at levelmanager is not checked and player have to load game by manually
    /// </summary>
    [Button("Load Level")]
    public void LoadLevel()
    {
        _levelManager.LoadLevel(_gameController.Level);
    }

}
