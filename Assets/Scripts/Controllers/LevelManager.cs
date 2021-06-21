using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public enum ManagerOptions { SingleScene, MultiScene }

    public ManagerOptions Mode;
    
    /// <summary>
    /// Triggered when new level is loaded
    /// </summary>
    [HideInInspector] public UnityEvent OnLevelLoaded=new UnityEvent();

    [Tooltip("If specified, when all levels are finished, game will start again from this level index." +
             " If specified -1, levels will be selected randomly after all levels are finished.")]
    public int RepeatLevelStart=-1;

    [Tooltip("If checked, level manager will load last played level automatically when game is started")]
    public bool AutoLoad=true;

    protected GameController _gameController;

    public int NumberOfTotalLevels { get; protected set; }
    
    
    #region Single Scene Parameters

    private bool IsSingleScene = false;

    [ShowIf("IsSingleScene")]public LevelData[] Levels;
    
    [Tooltip("Level data that will be loaded directly without checking levels")]
    [ShowIf("IsSingleScene")]public LevelData TestLevel;
    public LevelData ActiveLevelData{get; private set;}
    
    [Tooltip("Game will be tried to start with this level ignoring game data. This is useful for fast development")]
    [ShowIf("IsSingleScene")]public int TestLevelIndex=-1;

    /// <summary>
    /// Whether or not there are active laoded level in the scene
    /// </summary>
    public bool LevelIsLoaded => ActiveLevelData!=null;
    
    #endregion
    
    
    protected new void Awake(){
        base.Awake();
    }
    
    protected IEnumerator Start(){

        _gameController=GameController.request();

        if (Mode == ManagerOptions.MultiScene)
        {
            //-1 for initial scenes that will be used for analytics and controllers
            NumberOfTotalLevels = SceneManager.sceneCountInBuildSettings-2;
        
            //This script must be consistent within scenes
            DontDestroyOnLoad(this);
            SceneManager.sceneLoaded += OnNewSceneLoaded;
            
              
            //Prevent race condition
            yield return null;
        
            //If auto load is checked load level with current level automatically when game is started
        
            if (AutoLoad)
            {
                //Id build index is 1 load game data level
                if(SceneManager.GetActiveScene().buildIndex==1)
                    LoadLevel(_gameController.Level);
                //else it means game is in debug mode and current loaded level is the level that intended to play
                //in that case do nothing only trigger level loaded event
                else
                {
                    OnLevelLoaded.Invoke();
                }

                _gameController.onLevelChanged.AddListener(LoadLevel);
            }
            
            

        }else if (Mode == ManagerOptions.SingleScene)
        {
            //Error protection
            if(Levels.Length==0 && TestLevel==null){
                Debug.LogError("There is no level assigned to level manager. You have to assign level data to run the game.");
                Destroy(gameObject);
                yield break;
            }
        
            NumberOfTotalLevels = Levels.Length;

            
            yield return null;
        
            //If auto load is checked load level with current level automatically when game is started
            if (AutoLoad)
            {
                LoadLevel(_gameController.Level);
                _gameController.onLevelChanged.AddListener(LoadLevel);
            }
        }

       
    }
    
    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("new scene is laoded!");
        OnLevelLoaded.Invoke();
    }

    public void LoadLevel(int levelIndex)
    {
        if (Mode == ManagerOptions.MultiScene)
        {
            if (levelIndex < NumberOfTotalLevels)
            {
                SceneManager.LoadScene(levelIndex + 2);
            }
            else
            {
                //Else laod level with repeat level status
                bool randomRepeating = RepeatLevelStart == -1 || RepeatLevelStart >= NumberOfTotalLevels;
                
                if(randomRepeating){
                    SceneManager.LoadScene(Random.Range(0,NumberOfTotalLevels) + 2);
                }else{
                    
                    int deltaLevelIndex=levelIndex-NumberOfTotalLevels;
                    int repeatLevelIndex= RepeatLevelStart+ (deltaLevelIndex%(NumberOfTotalLevels-RepeatLevelStart));
                    SceneManager.LoadScene(repeatLevelIndex + 2);
                }
            }

        }else if (Mode == ManagerOptions.SingleScene)
        {
            ClearLoadedLevel();

            //First try to load test levels if debug mode is active
            if(_gameController.IsDebugMode && (TestLevelIndex!=-1 || TestLevel!=null)){


                if (TestLevel!=null){
                    LoadLevelWithData(TestLevel);
                }else{
                    TestLevelIndex=Mathf.Clamp(TestLevelIndex,0,Levels.Length);
                    LoadLevelWithData(Levels[TestLevelIndex]);
                }
            
            }else{

                //If level index is in range of level array capacity
                if(levelIndex<Levels.Length){
                
                    LoadLevelWithData(Levels[levelIndex]);
                
                }else{
                
                    //Else laod level with repeat level status
                    bool randomRepeating = RepeatLevelStart == -1 || RepeatLevelStart >= Levels.Length;
                
                    if(randomRepeating){
                        LoadLevelWithData(Levels[Random.Range(0,Levels.Length)]);
                    }else{
                    
                        int deltaLevelIndex=levelIndex-Levels.Length;
                        int repeatLevelIndex= RepeatLevelStart+ (deltaLevelIndex%(Levels.Length-RepeatLevelStart));
                        LoadLevelWithData(Levels[repeatLevelIndex]);
                    
                    }
                }
            }
            OnLevelLoaded.Invoke();
        }

    }
    
    /// <summary>
    /// Clears active if level if there is loaded one
    /// </summary>
    public void ClearLoadedLevel(){
        
        if (LevelIsLoaded)
        {
            //Clear all children object while all related objects will be spawned under this object
            for(int i=0;i<transform.childCount;i++)
                DestroyImmediate(transform.GetChild(i).gameObject);
            
            ActiveLevelData=null;
        }
        
    }

    private void LoadLevelWithData(LevelData levelData)
    {
        if (ActiveLevelData != null)
        {
            Debug.LogError("There is already active level. You have to clear active level before loading new level");
            return;
        }
        
        ActiveLevelData = levelData;
        
        //------- Example level loading. This part will be changed according to design of mechanic --------
        
        GameObject exampleLevelObject= Instantiate(levelData.splineComputer,transform);
        exampleLevelObject.GetComponent<MeshRenderer>().material=levelData.LevelMaterial;
        
        //------------------------------------------------------
    }

    private void OnValidate()
    {
        IsSingleScene = Mode == ManagerOptions.SingleScene;
    }
}
