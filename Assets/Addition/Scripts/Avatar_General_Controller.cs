using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar_General_Controller : MonoBehaviour
{

    Avatar_Controller[] allAvatars;
    [SerializeField] float startIncreaseAmount = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        allAvatars = FindObjectsOfType<Avatar_Controller>();

        GameController.request().OnGameStarted.AddListener(RestartAllPositions);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void RestartAllPositions()
    {
        float startPos = 0f;

        foreach(Avatar_Controller avatar in allAvatars)
        {
            if(avatar.enabled == false)
            {

                avatar.gameObject.SetActive(true);

            }

            avatar.RestartPosition(startPos);

            startPos += startIncreaseAmount;

        }





    }


}
