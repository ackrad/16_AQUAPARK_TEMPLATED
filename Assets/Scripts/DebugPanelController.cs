using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Used for switching between levels easily. Do not use this approach to load new levels it is only for debug purposes
/// </summary>
public class DebugPanelController : MonoBehaviour
{
    private GameController gameController;
    private LevelManager _levelManager;
    
    public Text debugLevelLabel;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameController.request();
        _levelManager = LevelManager.request();
        debugLevelLabel.text = gameController.Level.ToString();
    }
    

    public void resetGameData(){
        if(gameController.IsGameStarted) gameController.WinGame();
        gameController.ClearSaveData();
        _levelManager.LoadLevel(0);
        debugLevelLabel.text = 0.ToString();
    }
    
    public void increaseDebugLevelField()
    {
        if (int.TryParse(debugLevelLabel.text,out int result)) debugLevelLabel.text = (result + 1).ToString();
    }

    public void deacreseDebugLevelField()
    {
        if (int.TryParse(debugLevelLabel.text,out int result)) debugLevelLabel.text = (result - 1).ToString();
    }

    public void goDebugLevelField()
    {
        if (int.TryParse(debugLevelLabel.text,out int result))
        {
            if(gameController.IsGameStarted) gameController.WinGame();
            gameController.SetLevel(result);
        }
    }

}
