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
        if (AIon)
        {

            StartCoroutine(RandomDecisionMaking());
        }

        avatar_Controller.onWin.AddListener(WinGame);
        avatar_Controller.onLose.AddListener(LoseGame);
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



            avatar_Controller.player_Movement = (Avatar_Controller.MovementEnum)Random.Range(0, 3);

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

}
