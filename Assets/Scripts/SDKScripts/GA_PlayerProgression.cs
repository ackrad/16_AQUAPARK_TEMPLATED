using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GA_PlayerProgression : MonoBehaviour
{
/*
    public static GA_PlayerProgression instance;
    
    public bool muteInDebug=true;
    
    GameController gameController;
    
    void Awake()
    {
        if(instance!=null){
            Destroy(this);
            return;
        }else{
            instance=this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        GameAnalyticsSDK.GameAnalytics.Initialize();
        GameAnalyticsSDK.GameAnalytics.SettingsGA.NewVersion = Application.version;
        GameAnalyticsSDK.GameAnalytics.StartSession();
    
        Debug.Log("Game manager initialized");
    
    
        gameController=GameController.request();
        gameController.onGameStarted.AddListener(()=>{
            levelStarted(gameController.getGameData().level);
        });
        
        gameController.onGameCompleted.AddListener(()=>{
            levelEnd(gameController.getGameData().level);
        });
    }
    
    public void levelStarted(int levelIndex){
    
        levelIndex+=1;
        if(gameController.getGameData().levelStartEventLevels.Contains(levelIndex)){
            Debug.Log("Level start data of this level index is already send, So dont sending this one.");
            return;
        }
            
        Debug.Log("Sending level start event. Level index: "+(levelIndex));
        if((muteInDebug && Debug.isDebugBuild))
            return;
    
    
        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start,"Level "+(levelIndex));
        gameController.getGameData().addLevelEvent(levelIndex,GameData.EventType.Start);
        
    
    }
    
    public void levelEnd(int levelIndex){
    
        levelIndex+=1;
        
        if(gameController.getGameData().levelEndEventLevels.Contains(levelIndex)){
            Debug.Log("Level start data of this level index is already send, So dont sending this one.");
            return;
        }
    
        Debug.Log("Sending level end event. Level index: "+(levelIndex));
        if((muteInDebug && Debug.isDebugBuild))
            return;
    
        GameAnalyticsSDK.GameAnalytics.NewProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete,"Level "+(levelIndex));
        gameController.getGameData().addLevelEvent(levelIndex,GameData.EventType.End);
    
    }
    
    
    private void OnApplicationQuit() {
        GameAnalyticsSDK.GameAnalytics.EndSession();
    }
*/
}
