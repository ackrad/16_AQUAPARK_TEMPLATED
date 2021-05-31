using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRelease : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if(Debug.isDebugBuild==false){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
