using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using System;




public class Player_Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sideMoveSpeed = 5f;
    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float upForce = 10f;
    [SerializeField] float secondsToWait = 0.5f;

    [SerializeField] SplineComputer sp;
    [Header("TODO Bunu dynamic yap")]
    [SerializeField] float maxOfset = 3f;
    [SerializeField] string splineTag = "Spline";
    [SerializeField] Transform winPosition;
    // cached
    SplineFollower spFollower;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    
    //booleans
    bool isFollowing = false;
    public bool isPlayerActive = false;
    [Range(-1,1)] private float fingerMovement = 0;

    [Header("Ending Animation Metrics")]
    [SerializeField] float animMoveDuration=2f ;
    [SerializeField] Transform poolPosition;
    [SerializeField] float upMoveAmount = 7f;
    [SerializeField] PathType pathType;
    [SerializeField] PathMode pathMode;

    [Header("Game Manager")]
    [SerializeField] int coinWin = 100;
    private float coinTimer;

    CameraController cmController;
    GameController gameController;

    private void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe += SwipeHappened;
    }

    private void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerSwipe -= SwipeHappened;

    }


    // Start is called before the first frame update
    void Start()
    {
        // Eklentiler
        cmController = FindObjectOfType<CameraController>();

        gameController = GameController.request();
        
        //
        spFollower = GetComponent<SplineFollower>();
        spFollower.spline = FindObjectOfType<SplineComputer>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        coinTimer = 0f;
        gameController.OnGameStarted.AddListener(RestartPosition);




    }

    // Update is called once per frame
    void Update()
    {

        var fingers = Lean.Touch.LeanTouch.Fingers;


     
        if (isPlayerActive)
        {
            coinTimer += Time.deltaTime;
        }
        if (isFollowing  && isPlayerActive)
        {
            
                SlidingMethod();
            CheckIfOutOfOffset();

            // sp follower 0.99 a gelmiyor?
            if (sp.Project(transform.position).percent > 0.97)
            {
                Debug.Log("xd");


                WinGame();
            }

        }


        else
        {
            if (isPlayerActive)
            {
                FlyingMethod();
            }
        }


    }

    private void FlyingMethod()
    {


        rb.velocity = new Vector3(0,rb.velocity.y,0) + transform.forward * moveSpeed; // y velocityi koruyup ileri doðru hýz vermek için


        if (fingerMovement > 0)
        {
            Vector3 test = new Vector3(0, rotationSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(test * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

        }

        else if (fingerMovement < 0)
        {
            Vector3 test = new Vector3(0, -rotationSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(test * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

        }

    }

    private void CheckIfOutOfOffset()
    {
        float currentOffset = spFollower.motion.offset.x;
        if(currentOffset > maxOfset || currentOffset < -maxOfset)
        {

            StartCoroutine(StopFollowingSpline());

        }
    }

    private IEnumerator StopFollowingSpline()
    {
        fingerMovement = 0;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        
        spFollower.follow = false;
        rb.isKinematic = false;
        rb.AddForce(transform.up * upForce, ForceMode.VelocityChange);



        // Make rigidbody unkinematic so you can apply upwards foce
        isFollowing = false;
        spFollower.motion.offset = new Vector2(0, 0);
        capsuleCollider.enabled = false;

        yield return new WaitForSeconds(secondsToWait);

        capsuleCollider.enabled = true;
    }



    private void SlidingMethod()
    {
       


        float offsetX = spFollower.motion.offset.x;
        float newOffsetX;

        if ( fingerMovement>0)  
        {
            newOffsetX = offsetX + sideMoveSpeed*Time.deltaTime;
            float newOffsetY = maxOfset * Mathf.Cos(newOffsetX / maxOfset);

            spFollower.motion.offset = new Vector2(newOffsetX, -newOffsetY);

        }

        else if (fingerMovement<0) 
        {

            newOffsetX = offsetX - sideMoveSpeed* Time.deltaTime;
            float newOffsetY = maxOfset * Mathf.Cos(newOffsetX / maxOfset);

            spFollower.motion.offset = new Vector2(newOffsetX, -newOffsetY);


        }




    }





    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(splineTag) )
        {

            StartFollowing();
        }


        else
        {
            LoseGame();

        }
    }

    private void StartFollowing()
    {
        fingerMovement = 0;
        rb.isKinematic = true;
        spFollower.Restart(sp.Project(transform.position).percent); //restarts following from projected point
        spFollower.follow = true;
        isFollowing = true;
        spFollower.motion.offset = new Vector2(0, -maxOfset);
    }



    public void WinGame()
    {
        ChangeToDiveCamera();
        float depthOfPool = 4f;


        spFollower.follow = false;
        isPlayerActive = false;
        Vector3[] path = new Vector3[3];

        

        path[0]=transform.position - (transform.position - poolPosition.position )/2 + new Vector3(0,upMoveAmount,0);
        path[1] = poolPosition.position;
        path[2] = poolPosition.position + new Vector3(0, depthOfPool, 0);

        transform.DOPath(path, animMoveDuration, pathType, pathMode,10,Color.red).OnComplete(() => { ChangeToWinCamera(); gameController.WinGame(); gameController.EarnMoney(LevelManager.request().ActiveLevelData.Price); });

        


    }

    private void LoseGame()
    {

        gameController.LostGame();

    }

    private void ChangeToDiveCamera()
    {
        cmController.setCameraStatus(CameraStatus.Camera2);


    }

    private void ChangeToWinCamera()
    {
        transform.position = winPosition.position;

        cmController.setCameraStatus(CameraStatus.Camera3);

        // gameController.WinGame(coinWin - (int)coinTimer);

    }

    private void ChangeToSlideCamera()
    {

        cmController.setCameraStatus(CameraStatus.Camera1);


    }


    private void SwipeHappened(Lean.Touch.LeanFinger finger)
    {

        
        if(finger.SwipeScaledDelta.x > 0) //Swipe Right
        {
            fingerMovement += 1;

        }

        else if(finger.SwipeScaledDelta.x < 0 ) //Swipe Left
        {
            fingerMovement -= 1;
        }

    }




    private void SetSplineFollower()
    {
        spFollower.spline = FindObjectOfType<SplineComputer>();
        sp = FindObjectOfType<SplineComputer>();

    }

    private void RestartPosition()
    {
        SetSplineFollower();

        transform.position =sp.GetPoints()[0].position;

        spFollower.Restart(sp.Project(transform.position).percent); //restarts following from projected point
        spFollower.follow = true;
        isPlayerActive = true;
        isFollowing = true;
        ChangeToSlideCamera();


    }

    private void DisableThisComponent()
    {

        this.enabled = false;

    }

    private void EnableThisComponent()
    {

        this.enabled = true;
    }


   
}
