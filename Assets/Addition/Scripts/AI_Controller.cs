using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    //booleans 

    public bool AIon = true;

    private int decision;
    //cached variables
    Avatar_Controller avatar_Controller;

    // Start is called before the first frame update
    void Start()
    {
        avatar_Controller = GetComponent<Avatar_Controller>();
   

        avatar_Controller.onWin.AddListener(WinGame);
        avatar_Controller.onLose.AddListener(LoseGame);
        avatar_Controller.onWin.AddListener(TurnAIOff);
        avatar_Controller.onLose.AddListener(TurnAIOff);

        GameController.request().OnGameStarted.AddListener(StartAI);


    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator RandomDecisionMaking()
    {

        while (AIon)
        {
            yield return new WaitForSeconds(1f);



            avatar_Controller.player_Movement = (Avatar_Controller.MovementEnum)UnityEngine.Random.Range(0, 3);

        }

    }



    private void WinGame()
    {

        avatar_Controller.WinGameAI();

    }

    private void LoseGame()
    {

        avatar_Controller.LoseGameAI();

    }



    private void TurnAIOff()
    {

        AIon = false;

    }


    private void StartAI()
    {

        int behaviour = UnityEngine.Random.Range(0, 3);

        Debug.Log(behaviour);

        if (behaviour == 0)
        {

            return;
        }

        else if (behaviour == 1) // random ai
        {
            AIon = true;

            StartCoroutine(RandomDecisionMaking());
        }

        else if (behaviour == 2)
        {
            AIon = true;


            StartCoroutine(SlidingAI());


        }
    }

    private IEnumerator SlidingAI() // HORRIBLE CODING DO NOT DO THIS. 
    {

        while (AIon)
        {

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.left;

            yield return new WaitForSeconds(0.2f);

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.middle;


            yield return new WaitForSeconds(0.2f);

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.right;

            yield return new WaitForSeconds(0.2f);

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.right;

            yield return new WaitForSeconds(0.2f);

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.middle;


            yield return new WaitForSeconds(0.2f);

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.left;

            yield return new WaitForSeconds(0.2f);

        }

    }
}
