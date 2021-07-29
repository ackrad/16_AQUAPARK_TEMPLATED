using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinParticleStart : MonoBehaviour
{

    private void Start()
    {
        GameController.request().OnGameWin.AddListener(StartParticles);
    }

    private void StartParticles()
    {

        GetComponent<ParticleSystem>().Play();


    }




}
