using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnDebug : MonoBehaviour
{
    public bool destroyOnEditor=true;
    public bool destroyOnDebug=true;

    // Start is called before the first frame update
    public virtual void Start()
    {
        if((destroyOnDebug  &&Debug.isDebugBuild)|| (destroyOnEditor && Application.isEditor)){
            Destroy(gameObject);
        }
    }


}
