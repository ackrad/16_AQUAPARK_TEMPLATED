using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{

    public float speed = 100f;
    public Vector3 axis=new Vector3(0,1,0);
    public Transform pivot;
    
    // Start is called before the first frame update
    void Start()
    {
        if (pivot == null) pivot = transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(pivot.position,axis, speed*Time.fixedDeltaTime);
    }
}
