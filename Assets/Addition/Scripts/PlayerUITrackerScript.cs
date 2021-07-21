using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUITrackerScript : MonoBehaviour
{


    public Vector3 positionToFollow = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = Vector3.Lerp(transform.position, positionToFollow, Time.deltaTime);


    }


    public void ChangeFollowTarget(Transform newPos)
    {
        positionToFollow = newPos.position;



    }


}

