using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;



public class OnTrigger : MonoBehaviour
{

    public UnityEventGameObject onEnter;
    public UnityEventGameObject onExit;
    public bool destroyOnEnter=false;
    public bool destroyOnExit=false;

    public string[] tags;

    public bool printOnTrigger=false;

    List<Collider> ignoredColliders;

    public float delay=0;


    

    private void Awake() {
        if(ignoredColliders==null)
            ignoredColliders=new List<Collider>();
    }

    private void Update() {
        while(delay>0)
            delay-=Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {

        // if(delay>0)
        //     return;
        
        if(ignoredColliders!=null && ignoredColliders.Contains(other))
            return;

        if(tags.Contains(other.gameObject.tag)){

            if(printOnTrigger){
                Debug.Log(other.gameObject.name+" entered to tiggger");
            }
            
            if(onEnter!=null)
                onEnter.Invoke(other.gameObject);

            if(destroyOnEnter){
                Destroy(this);
                Destroy(gameObject);
            }
          
        }
        
    }

    
    private void OnTriggerExit(Collider other) {
            
        if(delay>0)
            return;

              
        if(ignoredColliders!=null && ignoredColliders.Contains(other))
            return;

        if(tags.Contains(other.gameObject.tag)){
            
            if(onExit!=null)
                onExit.Invoke(other.gameObject);
        
            if(destroyOnExit)
                Destroy(gameObject);

            
            if(printOnTrigger){
                Debug.Log(other.gameObject.name+" exit from tiggger");
            }
        }
    }

    public void addIgnoredCollider(Collider col){
        if(ignoredColliders==null)
            ignoredColliders=new List<Collider>();
    }

}
