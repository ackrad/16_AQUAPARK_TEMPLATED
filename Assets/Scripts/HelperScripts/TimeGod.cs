using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGod : MonoBehaviour
{
    [Range(0,1)]
    public float timeScale=1;


    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale!=timeScale)
            Time.timeScale=timeScale;
    }
}
