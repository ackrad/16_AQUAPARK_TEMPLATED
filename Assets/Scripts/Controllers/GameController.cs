using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


//Game Controller can be used for stage and level logic. 
//It creates comminication within game data.
//It includes events that can be used for building good communication
public class GameController : Singleton<GameController>
{

    private const string level_save_key = "game_level";
    private const string money_save_key = "main_money";
    
    //Weather or not gameplay is started
    public bool IsGameStarted{private set; get;} = false;

    //On game started and finished events
    //They will triggered when game (play time) is started and finished
    //It can derive according to design
    [FormerlySerializedAs("onGameStarted")] [HideInInspector]public UnityEvent OnGameStarted=new UnityEvent();
    [FormerlySerializedAs("onGameWin")] public UnityEvent OnGameWin=new UnityEvent();
    [FormerlySerializedAs("onGameLost")] public UnityEvent OnGameLost=new UnityEvent();
    [FormerlySerializedAs("onGameRestarted")] [HideInInspector]public UnityEvent OnGameRestarted=new UnityEvent();

    //Events that will be triggered when level are changed
    [HideInInspector]public UnityEventInt onLevelChanged=new UnityEventInt();
    
    [HideInInspector]public UnityEventFloat onMoneyChanged=new UnityEventFloat();

    //Current score of player
    public float Money { private set; get; }
    public int Level { private set; get; }
    
    public bool AutoSave = true;
    
    public bool IsDebugMode=true;

    [Tooltip("If checked, start state will be triggered automatically when a level loaded")]
    public bool AutoStart=true;

    [Tooltip("If true game data will erased when game is updated in devices")]
    public bool ClearDataForOldVersions=true;

    public int LockFrameRate = 60;
    
    private float fpsDeltaTime = 0.0f;

    public LeanTouch InputController { get; private set; }



    protected override void Awake()
    {
        base.Awake();
       
	    Screen.orientation = ScreenOrientation.Portrait;
        
        //Try to get debug mode from remote directly (dont use elephant controller because it is not initialized yet!
        IsDebugMode =ElephantSDK.RemoteConfig.GetInstance().GetBool("debugMode", IsDebugMode);

        string lastSaveVersion = PlayerPrefs.GetString("last_save_version","");

        if (lastSaveVersion == "" || ClearDataForOldVersions == false)
        {
            //Update last save version
            PlayerPrefs.SetString("last_save_version", Application.version);
        }
        else if (lastSaveVersion != Application.version)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetString("last_save_version", Application.version);
        }
        
        
        Level = PlayerPrefs.GetInt(level_save_key, 0);
        Money = PlayerPrefs.GetInt(money_save_key, 0);


        InputController = GetComponentInChildren<LeanTouch>();
        //by default input controller is closed because it will be active only on gameplay
        if(InputController) InputController.enabled = false;
        
        Application.targetFrameRate=LockFrameRate;
        QualitySettings.vSyncCount = 0;
        
        //Call the function at each time another scene is loaded
        SceneManager.sceneLoaded += OnNewSceneLoaded;
        DontDestroyOnLoad(this);
        
        

    }

    private void Start(){
        //If auto start is checked add start game functionality to on level loaded event 
        if(AutoStart){
            LevelManager.request().OnLevelLoaded.AddListener(StartGame);
        }
        
    }
    
    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Will use this function to trigger load functionality if game is using multiple scene 
    }
    
    /// <summary>
    /// Use this method to start gameplay
    /// </summary>
    public virtual void StartGame()
    {
        //If game is started already cant start game again
        if (IsGameStarted)
        {
            Debug.LogWarning("Game is already started. You can not start the game after it is started already");
            return;
        }
        
        OnGameStarted.Invoke();
        IsGameStarted = true;

        if (InputController) InputController.enabled = true;
    }
    
    
    /// <summary>
    /// Use this method to trigger win state of the game
    /// </summary>
    public virtual void WinGame()
    {
        if (!IsGameStarted)
        {
            Debug.LogError("Gameplay is not started you can not trigger win game action");
            return;
        }
        
        OnGameWin.Invoke();
        //Set game is started boolean
        IsGameStarted = false;
        
        if (InputController) InputController.enabled = false;
    }

    /// <summary>
    /// Use this method to trigger lost state of the game
    /// </summary>
    public virtual void LostGame(){
        if (!IsGameStarted)
        {
            Debug.LogError("Gameplay is not started you can not trigger lost game action");
            return;
        }
        
        OnGameLost.Invoke();
        IsGameStarted=false;
        
        if (InputController) InputController.enabled = false;
    }
    
    public virtual void EarnMoney(float amount)
    {
        Money += amount;
        onMoneyChanged.Invoke(Money);
        PlayerPrefs.SetFloat(money_save_key,Money);
    }

    //Spends money as amount. If provided amount is too much, it will return false else it will return true
    public virtual bool SpendMoney(float amount)
    {
        if (amount <= Money)
        {
            Money -= amount;
            onMoneyChanged.Invoke(Money);
            PlayerPrefs.SetFloat(money_save_key,Money);
            return true;
        }
        
        return false;
        
    }
    
    public void OnDestroy()
    {
        //Remove On new scene loaded delegate from scene manager
        SceneManager.sceneLoaded -= OnNewSceneLoaded;

        if (!AutoSave)
            PlayerPrefs.DeleteAll();
        
    }
    

    [Button("Clear Save Data")]
    public virtual void ClearSaveData(){
        PlayerPrefs.DeleteAll();
    }

    public virtual void IncreaseLevel(){

        if (IsGameStarted)
        {
            Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
            return;
        }


        Level++;
        PlayerPrefs.SetInt(level_save_key,Level);
        onLevelChanged.Invoke(Level);
        
    }

    /// <summary>
    /// Dont use this function it is only used for debugging!!!!
    /// </summary>
    /// <param name="levelIndex"></param>
    public void SetLevel(int levelIndex)
    {
        Level = levelIndex;
        PlayerPrefs.SetInt(level_save_key,Level);
        onLevelChanged.Invoke(Level);
    }
    
    public virtual void ReloadLevel(){
        
        if (IsGameStarted)
        {
            Debug.LogError("Gameplay is continuing You can not increase level during a gameplay");
            return;
        }
        
        onLevelChanged.Invoke(Level);
        OnGameRestarted.Invoke();
    }
    

    private void OnGUI() {
        if (IsDebugMode)
        {
            showVersion();
            showFPS();
            showGameStatus();
        }
    }

    void Update()
    {
        if(IsDebugMode) fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;
    }

    private void showVersion(){
        
        int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0.95f*h, w, h * 0.02f);
		style.alignment = TextAnchor.LowerCenter;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
   	        var gameVersion=Application.version;
		string text = "v "+gameVersion;
		GUI.Label(rect, text, style);
    }

    
    private void showFPS()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        float msec = fpsDeltaTime * 1000.0f;
        float fps = 1.0f / fpsDeltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
        
    }

    private void showGameStatus()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(-0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperRight;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
        string text = "Gameplay is started: "+IsGameStarted;
        GUI.Label(rect, text, style);

    }

}


