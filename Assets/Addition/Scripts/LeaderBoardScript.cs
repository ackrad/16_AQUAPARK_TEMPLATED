using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;


public class LeaderBoardScript : MonoBehaviour
{



        //  TODO TURN THIS INTO LIST
    [SerializeField] Avatar_Controller[] players = new Avatar_Controller[4];

    [SerializeField] RectTransform[] texts = new RectTransform[4];

    [SerializeField] GameObject leaderBoardCanvas;

    Dictionary<GameObject, float> myDict = new Dictionary<GameObject, float>();

    Vector3 firstPlaceUIPos = new Vector3(300f, -50f, 0f);

    Vector3 uiLeaderBoardoffset =new Vector3(0f, -25f,0);

    // Start is called before the first frame update
    void Start()
    {

        UIController.request().StartSliding.AddListener(StartLeaderBoard);

        GameController.request().OnGameLost.AddListener(StopLeaderBoard);
        GameController.request().OnGameWin.AddListener(StopLeaderBoard);

        // assigning player names to boxes

        int loopvar = 0;
        foreach (Avatar_Controller player in players)
        {
            texts[loopvar].GetComponentInChildren<Text>().text = player.name;
            loopvar++;


        }

    }



    private void StartLeaderBoard()
    {
        StartCoroutine(LeaderBoardUpdate());

        leaderBoardCanvas.SetActive(true);



    }

    private IEnumerator LeaderBoardUpdate()
    {
        while (GameController.request().IsGameStarted)
        {

            UpdateLeaderBoard();
            yield return new WaitForSeconds(2f);


        }
    }


    

    private void UpdateLeaderBoard()
    {

        myDict.Clear();
        foreach (Avatar_Controller player in players)
        {
            float distanceToPool = player.DistanceToPool();

            myDict.Add(player.gameObject, distanceToPool);


        }


        var sortedDict = from entry in myDict orderby entry.Value ascending select entry;

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {

                if(sortedDict.Skip(i).First().Key.name == texts[j].GetComponentInChildren<Text>().text  )
                {

                    Vector3 movePos = firstPlaceUIPos + i * uiLeaderBoardoffset;

                    

                    texts[j].DOLocalMove(movePos, 1);

                   
                }



            }


    texts[i].GetComponentInChildren<Text>().text = sortedDict.Skip(i).First().Key.name;

        }



    }

    private void StopLeaderBoard()
    {
        leaderBoardCanvas.SetActive(false);



    }
}
