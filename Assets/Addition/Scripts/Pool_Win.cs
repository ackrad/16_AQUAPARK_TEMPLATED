using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
public class Pool_Win : MonoBehaviour
{

    private void Start()
    {
        GameController.request().OnGameStarted.AddListener(RestartPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Player_Controller>().isPlayerActive)
        {

            other.GetComponent<Player_Controller>().WinGame();
        }
    }

    private void RestartPosition()
    {
        SplineComputer sp = FindObjectOfType<SplineComputer>();

        Vector3 distance = sp.GetPoint(sp.GetPoints().Length - 1).position - sp.GetPoint(sp.GetPoints().Length - 2).position;

        distance = distance.normalized;

        transform.position = sp.GetPoint(sp.GetPoints().Length - 1).position - new Vector3(0f,15f,0f) + distance * 2f; //TODO fix magic number



    }

}
