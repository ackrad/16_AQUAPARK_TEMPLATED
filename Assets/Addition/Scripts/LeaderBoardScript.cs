using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using DG.Tweening;


public class LeaderBoardScript : MonoBehaviour
{

    [SerializeField] Avatar_General_Controller avatarGeneral;
    [SerializeField] Transform listContent;
    [SerializeField] GameObject gameObjectToAddToList;
    [SerializeField] GameObject playerTextPreFab;

    //  TODO TURN THIS INTO LIST
    
    [SerializeField] Avatar_Controller[] players;

    private int leaderboardLength;
    private List<GameObject> posGameObjects = new List<GameObject>();
    private List<GameObject> texts = new List<GameObject>();

    [SerializeField] GameObject leaderBoardCanvas;

    Dictionary<GameObject, float> myDict = new Dictionary<GameObject, float>();



    // Start is called before the first frame update
    void Start()
    {

        UIController.request().StartSliding.AddListener(StartLeaderBoard);

        GameController.request().OnGameLost.AddListener(StopLeaderBoard);
        GameController.request().OnGameWin.AddListener(StopLeaderBoard);

        // assigning player names to boxes

      
        
    }


    private void StartLeaderBoard()
    {

        StartCoroutine(_StartLeaderBoard());

    }




    private IEnumerator _StartLeaderBoard()
    {
        leaderboardLength = avatarGeneral.ReturnAvatarCount();
        posGameObjects.Clear();
        texts.Clear();

        for(int i =0; i < leaderboardLength; i++)
        {

            var childObject =  Instantiate(gameObjectToAddToList,listContent);

            posGameObjects.Add(childObject);


            

        }

        yield return null;
        
        for (int i = 0; i < leaderboardLength; i++)
        {

            var childTextObject = Instantiate(playerTextPreFab, posGameObjects[i].transform.position, Quaternion.identity, this.transform);
            childTextObject.GetComponentInChildren<Text>().text = players[i].name;


            texts.Add(childTextObject);

            texts[i].GetComponent<PlayerUITrackerScript>().ChangeFollowTarget(posGameObjects[i].transform);
        }




        leaderBoardCanvas.SetActive(true);


        StartCoroutine(LeaderBoardUpdate());




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

        for (int i = 0; i < leaderboardLength; i++)
        {
            for(int j=0; j< leaderboardLength; j++)
            {
                if(texts[j].GetComponentInChildren<Text>().text == sortedDict.Skip(i).First().Key.name)
                {

                    texts[j].GetComponent<PlayerUITrackerScript>().ChangeFollowTarget(posGameObjects[i].transform);

                }


            }
             



        }



        }



    

    private void StopLeaderBoard()
    {
        DestroyTrackers();
        leaderBoardCanvas.SetActive(false);



    }



    private void DestroyTrackers()
    {
        foreach(GameObject posGameObject in posGameObjects)
        {


            Destroy(posGameObject);
        }

        foreach (GameObject text in texts)
        {


            Destroy(text);
        }


    }
}
