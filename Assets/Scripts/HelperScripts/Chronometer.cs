using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Chronometer : MonoBehaviour
{

    public bool RunOnGameplay = true;
    [UnityEngine.UI.Extensions.ReadOnly]public float Timer;
    public bool PrintTimer=false;
    
    private bool isActive;



#if UNITY_EDITOR
    
    
    private void Start()
    {
        if (RunOnGameplay)
        {
            GameController gameController = GameController.request();
            gameController.OnGameStarted.AddListener(StartChronometer);
            gameController.OnGameWin.AddListener(PauseChronometer);
            gameController.OnGameLost.AddListener(PauseChronometer);
            
            LevelManager.Instance.OnLevelLoaded.AddListener(StopChronometer);
        }
            
    }
    
    // Update is called once per frame
    protected virtual void Update()
    {
        if(isActive){
            Timer+=Time.deltaTime;
            
            if(PrintTimer)
                print(Timer);
        }
    }
#endif
    [Button("Start")]
    public void StartChronometer(){
        isActive=true;
    }

    [Button("Pause")]
    public void PauseChronometer()
    {
        isActive = false;
    }
    
    [Button("Stop")]
    public void StopChronometer(){
        Timer=0;
        isActive = false;
    }

}
