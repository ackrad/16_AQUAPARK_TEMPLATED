using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ElephantSDK;


public class ElephantController : Singleton<ElephantController>
{
    
    private GameController _gameController;
    private Dictionary<string,string> remoteConfigData;
    
    protected override void Awake()
    {
        base.Awake();
        remoteConfigData = new Dictionary<string, string>();
    }

    public void Start()
    {
        _gameController = GameController.request();
        _gameController.OnGameStarted.AddListener(gameStarted);
        _gameController.OnGameWin.AddListener(gameCompleted);
        _gameController.OnGameLost.AddListener(gameFailed);
        
    }

    private void gameStarted()
    {
        int levelIndex = _gameController.Level+1;
        Elephant.LevelStarted(levelIndex);
        Debug.Log("Start event sent to elephant");
    }

    private void gameCompleted()
    {
        int levelIndex = _gameController.Level+1;
        Elephant.LevelCompleted(levelIndex);
        Debug.Log("Start complete sent to elephant");
    }

    private void gameFailed()
    {
        int levelIndex = _gameController.Level+1;
        Elephant.LevelFailed(levelIndex);
        Debug.Log("Start failed sent to elephant");
      
    }

    public float remoteFloatVariable(string key, float variable)
    {
        string defaultValue = variable.ToString();
        float.TryParse(RemoteConfig.GetInstance().Get(key, defaultValue),out variable);
        addRemoteInfo(key,variable.ToString());
        return variable;
    }

    public float remoteIntegerVariable(string key, int variable)
    {

        variable = RemoteConfig.GetInstance().GetInt(key, variable);
        addRemoteInfo(key,variable.ToString());
        return variable;
    }

    
    public string remoteStringVariable(string key, string variable)
    {
        string result= RemoteConfig.GetInstance().Get(key, variable);
        addRemoteInfo(key,variable.ToString());
        return result;
    }

    public bool remoteBoolVariable(string key, bool variable)
    {
        bool result= RemoteConfig.GetInstance().GetBool(key, variable);
        addRemoteInfo(key,variable.ToString());
        return result;
    }
    
    public static float RemoteFloatVariable(string key, float variable)
    {
        variable = RemoteConfig.GetInstance().GetFloat(key, variable);
        return variable;
    }
    
    
    public static int RemoteIntegerVariable(string key, int variable)
    {
        variable = RemoteConfig.GetInstance().GetInt(key, variable);
        return variable;
    }
    

    public static string RemoteStringVariable(string key, string variable)
    {
        string result= RemoteConfig.GetInstance().Get(key, variable);
        return result;
    }

    public static bool RemoteBoolVariable(string key, bool variable)
    {
        bool result= RemoteConfig.GetInstance().GetBool(key, variable);
        return result;
    }



    private void addRemoteInfo(string key, string value)
    {
        remoteConfigData[key] = value;
    }
    
    private void OnGUI() {
        
        if (_gameController.IsDebugMode && remoteConfigData.Count>0)
        {
            int w = Screen.width, h = Screen.height;
 
            GUIStyle style = new GUIStyle();
 
            Rect rect = new Rect(0, 0, w, h);
            style.alignment = TextAnchor.UpperRight;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);

            string text = "Remote Parameters: ";

            foreach (string configKey in remoteConfigData.Keys)
            {
                string value = remoteConfigData[configKey];
                text+="\n";
                text += configKey+": "+value;
            }
            
            GUI.Label(rect, text, style);
            
            
        }
            
    }
    


}
