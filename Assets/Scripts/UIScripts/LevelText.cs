using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Retrieve current level from game controller and assign it to text component of gameObject
/// </summary>
public class LevelText : TextController
{
    GameController gameController;
    
    private void Start() {
        gameController=GameController.request();
        
        if(gameController==null) return;
        gameController.onLevelChanged.AddListener(levelIsChanged);
        
        levelIsChanged(gameController.Level);
    }

    private void levelIsChanged(int level)
    {
        level++;
        updateText(level.ToString());
    }

    private void OnDestroy() {
        gameController.onLevelChanged.RemoveListener(levelIsChanged);
    }

}
