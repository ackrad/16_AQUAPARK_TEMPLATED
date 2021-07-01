using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Win : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Player_Controller>().isPlayerActive)
        {

            other.GetComponent<Player_Controller>().WinGame();
        }
    }

}
