using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : TextController
{
    
    GameController gameController;
    
    
    private void Start() {
        gameController=GameController.request();
        if(gameController==null) return;
        gameController.onMoneyChanged.AddListener(moneyIsChanged);
        moneyIsChanged(gameController.Money);
    }

    private void moneyIsChanged(float amount){
        updateText(amount.ToString());
        
    }

    private void OnDestroy() {
        if(gameController) 
            gameController.onMoneyChanged.RemoveListener(moneyIsChanged);
    }
}
