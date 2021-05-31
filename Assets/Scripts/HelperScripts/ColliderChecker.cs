using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks weather or not there is a collider with provided colliderLayer inside radius
public class ColliderChecker : MonoBehaviour
{
    public LayerMask colliderLayer;
    public GameObject colliderTrigger;
    public float checkRadius=1f;
    public bool isThereCollider =false;

    // Start is called before the first frame update
    void Start()
    {
        if(colliderTrigger==null){
            colliderTrigger=gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var overlayColliders=Physics.OverlapSphere(colliderTrigger.transform.position,checkRadius,colliderLayer);
        isThereCollider=overlayColliders.Length>0;
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        var pos=colliderTrigger!=null ? colliderTrigger.transform.position:transform.position;
        Gizmos.DrawSphere(pos, checkRadius);
    }
}
