using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DynamicObstacle : MonoBehaviour
{
    public enum Types{Movement,Rotation};

    public Types movementType=Types.Movement;
    public bool reversed=false;

    [Header("Movement Variables")]

    public float movementDistance=10f;
    public float movementSpeed=3f;
    public Vector3 movementAxis=Vector3.right;
    public Space movementSpace=Space.World;

    [Range(0,1)]
    public float movementRatio=0;



    [Header("Rotation Variables")]
    public bool pingPong=false; 
    public Vector3 orbitDirection=Vector3.up;
    public float rotateAngle=30f;
    public float orbitDistance=5f;
    public float rotateSpeed=1f;
    public bool lookAtOrbit=false;
    Vector3 orbitCenter;
    public float angle=0;
    float timeOffset=0;

    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        orbitCenter=transform.position+orbitDirection*orbitDistance;
        initialPosition=transform.position;

        movementAxis=movementAxis.normalized;

        if(movementType==Types.Rotation){
            timeOffset=Mathf.Asin((angle/(rotateAngle/2)));
            if(reversed)
                timeOffset-=Mathf.PI;
        }else{
            // timeOffset=Mathf.Asin((angle/(rotateAngle/2)));
            // if(reversed)
            //     timeOffset-=Mathf.PI;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        if(movementType==Types.Movement)
            updatePosition();
        else
            updateRotation();
            
    }

    void updateRotation(){
        if(!pingPong){
            angle += Time.deltaTime*rotateSpeed * (reversed? -1:1);
        }else{
            angle = rotateAngle * Mathf.Sin((Time.time+timeOffset) * rotateSpeed)/2;
        }

        Vector3 position=orbitCenter-Quaternion.AngleAxis(angle, Vector3.forward)*orbitDirection*orbitDistance;
        transform.position=position;
        if(lookAtOrbit)
            transform.up=orbitCenter-transform.position;
    }

    void updatePosition(){

        Vector3 movAxis = movementSpace==Space.World ? movementAxis : transform.TransformDirection(movementAxis);

        Vector3 p1=initialPosition+movAxis*movementDistance/2;
        Vector3 p2=initialPosition-movAxis*movementDistance/2;

        movementRatio= (Mathf.Sin((timeOffset+ Time.time)*movementSpeed)+1)/2;
        transform.position=Vector3.Lerp(p1,p2,movementRatio);
    }


    void OnDrawGizmos()
    {
        #if UNITY_EDITOR

        if(Application.isPlaying==false)
            initialPosition=transform.position;

        if(movementType==Types.Rotation){
    
         

            if(!pingPong)
                rotateAngle=360; 
            else   
                angle=Mathf.Clamp(angle,-rotateAngle/2,rotateAngle/2);

            if(orbitDistance<=0)
                orbitDistance=0.001f;
          
            orbitCenter=initialPosition+orbitDirection*orbitDistance;         
            UnityEditor.Handles.DrawWireArc(orbitCenter,Vector3.forward, Quaternion.AngleAxis(-rotateAngle/2, Vector3.forward)*(-1*orbitDirection),rotateAngle,orbitDistance);
            Gizmos.DrawSphere(orbitCenter-Quaternion.AngleAxis(angle, Vector3.forward)*orbitDirection*orbitDistance,0.5f);
                
        }else{
       

            movementAxis=movementAxis.normalized;

            Vector3 movAxis = movementSpace==Space.World ? movementAxis : transform.TransformDirection(movementAxis);
            
            Vector3 p1=initialPosition+movAxis*movementDistance/2;
            Vector3 p2=initialPosition-movAxis*movementDistance/2;
            Gizmos.DrawLine(p1,p2);
            Gizmos.DrawSphere(Vector3.Lerp(p1,p2,movementRatio),0.5f);
        }

        #endif
    }


}
