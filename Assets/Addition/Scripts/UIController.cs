using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class UIController : Singleton<UIController>
{

    public UnityEvent StartSliding = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        GameController.request().OnGameStarted.AddListener(InvokeStartSlideing);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InvokeStartSlideing()
    {

        StartCoroutine(CountDown());
    }



    private IEnumerator CountDown()
    {

        yield return new WaitForSeconds(1f); // TODO bunu serialized yapmak isteyebilirsin



        StartSliding.Invoke();


    }
}
