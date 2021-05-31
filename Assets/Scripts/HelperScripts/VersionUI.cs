using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VersionUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Debug.isDebugBuild==false)
            Destroy(gameObject);

        var version=Application.version;
        GetComponent<Text>().text="v "+version;
    }


}
