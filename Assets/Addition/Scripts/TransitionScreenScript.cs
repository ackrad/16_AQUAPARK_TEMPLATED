using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreenScript : MonoBehaviour
{

    [SerializeField] float loadTime = 2f;
    [SerializeField] Slider slider;
    float time = 0f;
    // Start is called before the first frame update


    private void Start()
    {

       // GameController.request().onLevelChanged.AddListener(StartLoadingScene);
    }


    private IEnumerator LoadingScene()
    {
        time = 0f;

        while (time<= loadTime)
        {
            time += Random.Range(0, 0.12f);
            slider.value = time;

            yield return new WaitForSeconds(0.1f);



        }

        Debug.Log("xd");


    }



    private void StartLoadingScene()
    {
        this.enabled = true;
        StartCoroutine(LoadingScene());

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            StartCoroutine(LoadingScene());
        }
    }

}
