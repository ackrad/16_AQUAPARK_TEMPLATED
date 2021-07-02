using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] int countdowntimer = 3;
    [SerializeField] Text countdownDisplay;

    // Start is called before the first frame update
   


    public void StartCountdown()
    {

        StartCoroutine(CountdownToStart());



    }


    IEnumerator CountdownToStart()
    {
        while(countdowntimer > 0)
        {
            countdownDisplay.text = countdowntimer.ToString();
            yield return new WaitForSeconds(1f);

            countdowntimer--;



        }

        countdownDisplay.text = "GO!";


    }
}
