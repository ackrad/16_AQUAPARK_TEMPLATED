using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

    GameController gameController;
    
    public GameObject InGameUI;
    public GameObject LevelEndUI;

    public GameObject LevelWinPanel;
    public GameObject LevelLostPanel;
    
    public Button NextLevelButton;
    public Button RestartLevelButton;

    [HideInInspector] public Canvas mainCanvas;


    // EGE KISMI
    [SerializeField] GameObject LoadSliderUI;
    [SerializeField] Slider slider;
    [SerializeField] float loadTime =1f;
    [SerializeField] Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameController=GameController.request();

        mainCanvas = GetComponent<Canvas>();
        
        gameController.OnGameWin.AddListener(GameIsWin);
        gameController.OnGameLost.AddListener(GameIsLost);
        
        gameController.OnGameStarted.AddListener(GameIsStarted);
        gameController.OnGameRestarted.AddListener(GameIsStarted);
        
        NextLevelButton.onClick.AddListener(gameController.IncreaseLevel);
        RestartLevelButton.onClick.AddListener(gameController.ReloadLevel);
        
        NextLevelButton.gameObject.SetActive(false);
        RestartLevelButton.gameObject.SetActive(false);
        
        LevelWinPanel.gameObject.SetActive(false);
        LevelLostPanel.gameObject.SetActive(false);

        //EGE KISMI YİNE
        NextLevelButton.onClick.AddListener(LoadAnimation);


        //Ege Kısmı Bitti
        //Event listerers may not catch start trigger of gamecontroller due to race condition there for check is game started boolean
        if(gameController.IsGameStarted) 
            GameIsStarted();
    }

    private void GameIsWin()
    {
        InGameUI.SetActive(false);
        LevelEndUI.SetActive(true);

        LevelWinPanel.gameObject.SetActive(true);
        NextLevelButton.gameObject.SetActive(true);
        
    }
    
    //Called when game time is finished (For both win or lost condition)
    private void GameIsLost()
    {
        InGameUI.SetActive(false);
        LevelEndUI.SetActive(true);
        
        LevelLostPanel.gameObject.SetActive(true);
        RestartLevelButton.gameObject.SetActive(true);
        
        
    }
    
    //Called when game time is started    
    private void GameIsStarted()
    {
        //Just in case 
        NextLevelButton.gameObject.SetActive(false);
        RestartLevelButton.gameObject.SetActive(false);
        
        LevelWinPanel.gameObject.SetActive(false);
        LevelLostPanel.gameObject.SetActive(false);
        
        InGameUI.SetActive(true);
        LevelEndUI.SetActive(false);
    }
    

    // EGEEEEEE

    private void LoadAnimation()
    {
        LoadSliderUI.SetActive(true);

        StartCoroutine(LoadingScene());
        StartCoroutine(ChangeLoadText());

    }

    private IEnumerator LoadingScene()
    {
        float time = 0f;

        while (time <= loadTime)
        {
            time += Random.Range(0, 0.12f);
            slider.value = time/loadTime;

            yield return new WaitForSeconds(0.1f);



        }

        LoadSliderUI.SetActive(false);


    }


    private IEnumerator ChangeLoadText()
    {
        float timeBetweenDots = 0.1f;
        int i = 0;
        while (true)
        {
            if (i <= 2)
            {
                text.text = text.text + ".";
                i++;
            }

            else
            {
                text.text = "LOADING";
                i = 0;

            }
            yield return new WaitForSeconds(timeBetweenDots);


        }


    }
}
