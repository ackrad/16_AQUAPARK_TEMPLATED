using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardScript : MonoBehaviour
{



        //  TODO TURN THIS INTO LIST
        [SerializeField] Avatar_Controller[] players = new Avatar_Controller[4];

    [SerializeField] GameObject leaderBoardCanvas;

    float[,] distances = new float[4,2];


    // Start is called before the first frame update
    void Start()
    {

        UIController.request().StartSliding.AddListener(StartLeaderBoard);

        GameController.request().OnGameLost.AddListener(StopLeaderBoard);

        
    }

 

    private void StartLeaderBoard()
    {

        leaderBoardCanvas.SetActive(true);

        int i = 0;

        foreach(Avatar_Controller player in players)
        {
            float distanceToPool = player.DistanceToPool();

            distances[i,0] = distanceToPool;
            distances[i, 1] = i;
            i++;

        }




    }


    private void StopLeaderBoard()
    {
        leaderBoardCanvas.SetActive(false);



    }
}
