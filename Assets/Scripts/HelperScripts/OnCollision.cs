using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class onCollisionEnterEvent:UnityEvent<GameObject>{};
[System.Serializable]
public class onCollisionExitEvent:UnityEvent<GameObject>{};
public class OnCollision : MonoBehaviour
{
  
    public UnityEventGameObject onEnter;
    public UnityEventGameObject onExit;

    public string[] tags; 

    private void OnCollisionEnter(Collision other) {

        if(onEnter!=null && tags.Contains(other.gameObject.tag)){
            onEnter.Invoke(other.gameObject);
        }
    }

    
    private void OnCollisionExit(Collision other) {
        if(onExit!=null && tags.Contains(other.gameObject.tag)){
            onExit.Invoke(other.gameObject);
        }
    }


    
}
