using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;
using System;
using UnityEngine.Events;

public class Avatar_Controller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sideMoveSpeed = 5f;
    [SerializeField] float rotationSpeed = 60f;
    [SerializeField] float upForce = 10f;
    [SerializeField] float secondsToWait = 0.5f;
    [SerializeField] float maxMoveSpeed = 30f;
    [SerializeField] float maxVelocity = -9f;
    [SerializeField] SplineComputer sp;
    [Header("TODO Bunu dynamic yap")]
    [SerializeField] float maxOfset = 3f;
    [SerializeField] float collisionSpeedIncrease = 5f;
    [SerializeField] ParticleSystem winParticle;


    private const string splineTag = "Spline";


    Transform winPosition;
    [SerializeField] AnimatorControllerScript animations;

    // cached
    SplineFollower spFollower;
    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    
    //booleans
    bool isFollowing = false;
    public bool isPlayerActive = false;

    [Header("Ending Animation Metrics")]
    [SerializeField] float animMoveDuration=2f ;
    Transform poolPosition;
    [SerializeField] float upMoveAmount = 7f;
    [SerializeField] PathType pathType;
    [SerializeField] PathMode pathMode;

    [Header("Game Manager")]
    [SerializeField] int coinWin = 100;
    private float coinTimer;

    CameraController cmController;
    GameController gameController;


    // Win And Lose Events
    [HideInInspector] public UnityEvent onLose = new UnityEvent();
    [HideInInspector] public UnityEvent onWin = new UnityEvent();



    public enum MovementEnum { right, left, middle };


    public MovementEnum player_Movement = MovementEnum.middle;

    // Start is called before the first frame update
    void Start()
    {
        cmController = FindObjectOfType<CameraController>();

        gameController = GameController.request();
        
        
        spFollower = GetComponent<SplineFollower>();
        spFollower.spline = FindObjectOfType<SplineComputer>();
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        coinTimer = 0f;
        animations.Slide();
        //UIController.request().StartSliding.AddListener(RestartPosition);
        gameController.OnGameStarted.AddListener(EnableThisObject);
        gameController.OnGameRestarted.AddListener(EnableThisObject);



        poolPosition = LevelManager.request().GetComponent<Required_Datas>().pool_Position;
        winPosition = LevelManager.request().GetComponent<Required_Datas>().win_Position;


    }

    private void FixedUpdate()
    {
        if(rb.velocity.y< maxVelocity)
        {

            rb.AddForce(new Vector3(0f, 9f, 0f));

        }

    }
    // Update is called once per frame
    void Update()
    {


        if (isPlayerActive)
        {
            coinTimer += Time.deltaTime;
        }


        if (isFollowing  && isPlayerActive)
        {
            
                SlidingMethod();
            CheckIfOutOfOffset();

            if (sp.Project(transform.position).percent > 0.97)
            {


                InvokeWinGameEvent();
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

    public void InvokeWinGameEvent()
    {
        onWin.Invoke();
    }

    private void FlyingMethod()
    {
    

        rb.velocity = new Vector3(0,rb.velocity.y,0) + transform.forward * moveSpeed; //keep y velocity
       
        
       

        if (player_Movement == MovementEnum.right)
        {
            Vector3 test = new Vector3(0, rotationSpeed, 0);
            Quaternion deltaRotation = Quaternion.Euler(test * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);

        }

        else if (player_Movement == MovementEnum.left)
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


            animations.Fall();
        }
    }


   
    private IEnumerator StopFollowingSpline()
    {
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

        //increase follow Speed
        IncreaseSlideSpeed();
      
        float offsetX = spFollower.motion.offset.x;
        float newOffsetX;

        if (player_Movement == MovementEnum.right)  
        {
            newOffsetX = offsetX + sideMoveSpeed*Time.deltaTime;
            float newOffsetY = maxOfset * Mathf.Cos(newOffsetX / maxOfset);

            spFollower.motion.offset = new Vector2(newOffsetX, -newOffsetY);

        }

        else if (player_Movement == MovementEnum.left)
        {

            newOffsetX = offsetX - sideMoveSpeed* Time.deltaTime;
            float newOffsetY = maxOfset * Mathf.Cos(newOffsetX / maxOfset);

            spFollower.motion.offset = new Vector2(newOffsetX, -newOffsetY);


        }




    }

    private void IncreaseSlideSpeed()
    {
        if (spFollower.followSpeed < maxMoveSpeed)
        {
            spFollower.followSpeed += 1f * Time.deltaTime;
            moveSpeed = spFollower.followSpeed;

        }

        else if (spFollower.followSpeed > maxMoveSpeed)
        {
            spFollower.followSpeed -= 1f * Time.deltaTime;
            moveSpeed = spFollower.followSpeed;

        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        // TODO change tags into const string references
        if (collision.collider.CompareTag(splineTag) )
        {

            StartFollowing();
        }


        else if(collision.collider.CompareTag("Respawn"))
        {
            LoseGameEvent();

        }

        else if (collision.collider.CompareTag("Player"))
        {

            Vector3 fwd = transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(transform.position, fwd, 10))
            {
           
                spFollower.followSpeed -= collisionSpeedIncrease;
                moveSpeed = spFollower.followSpeed;
            }

            else
            {
                spFollower.followSpeed += collisionSpeedIncrease;
                moveSpeed = spFollower.followSpeed;

            }





        }
    }

    private void LoseGameEvent()
    {
        onLose.Invoke();
    }

    private void StartFollowing()
    {
        animations.Slide();
        rb.isKinematic = true;
        spFollower.Restart(sp.Project(transform.position).percent); //restarts following from projected point
        spFollower.follow = true;
        isFollowing = true;
        spFollower.motion.offset = new Vector2(0, -maxOfset);
    }



    public void WinGameUser()
    {
        ChangeToDiveCamera();
        float depthOfPool = 4f;


        spFollower.follow = false;
        isPlayerActive = false;
        Vector3[] path = new Vector3[3];

        

        path[0]=transform.position - (transform.position - poolPosition.position )/2 + new Vector3(0,upMoveAmount,0);
        path[1] = poolPosition.position;
        path[2] = poolPosition.position + new Vector3(0, depthOfPool, 0);

        transform.DOPath(path, animMoveDuration, pathType, pathMode,10,Color.red).OnComplete(() => { ChangeToWinCamera(); gameController.WinGame(); gameController.EarnMoney(LevelManager.request().ActiveLevelData.Price); animations.Victory(); });

        


    }
    public void WinGameAI()
    {
        float depthOfPool = 4f;


        spFollower.follow = false;
        isPlayerActive = false;
        Vector3[] path = new Vector3[3];



        path[0] = transform.position - (transform.position - poolPosition.position) / 2 + new Vector3(0, upMoveAmount, 0);
        path[1] = poolPosition.position;
        path[2] = poolPosition.position + new Vector3(0, depthOfPool, 0);

        transform.DOPath(path, animMoveDuration, pathType, pathMode, 10, Color.red).OnComplete(() => { ChangeToWinPositionAI(); animations.Victory(); });

        transform.position = new Vector3(0f, 0f, 0f);


    }
    public void LoseGameUser()
    {

        gameController.LostGame();

    }

    public void LoseGameAI()
    {
        isPlayerActive = false;
        GetComponent<AI_Controller>().AIon = false;

        transform.position = new Vector3(0f, 0f, 0f); // todo find a better fix
        //this.gameObject.SetActive(false);

    }

    private void ChangeToDiveCamera()
    {
        cmController.setCameraStatus(CameraStatus.Camera2);


    }

    private void ChangeToWinCamera()
    {
        transform.position = winPosition.position;
        transform.rotation = winPosition.rotation;

        cmController.setCameraStatus(CameraStatus.Camera3);

        Instantiate(winParticle, transform.position, Quaternion.identity);


    }

    private void ChangeToWinPositionAI()
    {

        transform.position = winPosition.position;
        transform.rotation = winPosition.rotation;
    }

    private void ChangeToSlideCamera()
    {

        cmController.setCameraStatus(CameraStatus.Camera1);


    }

    private void EnableThisObject()
    {
        this.gameObject.SetActive(true);


    }
  

    

    private void SetSplineFollower()
    {
        spFollower.spline = FindObjectOfType<SplineComputer>();
        sp = FindObjectOfType<SplineComputer>();

    }

    public void RestartPosition(float startPos)
    {
        if (GetComponent<AI_Controller>())
        {

            GetComponent<AI_Controller>().AIon = true;

        }

        SetSplineFollower();

        transform.position = sp.GetPoints()[0].position;

        RestartFollowSpeed();
        spFollower.follow = true;
        isPlayerActive = true;
        isFollowing = true;
        spFollower.Restart(startPos); //restarts following from projected point
        ChangeToSlideCamera();

        animations.Slide();
    }

    private void RestartFollowSpeed()
    {
        spFollower.followSpeed = 5f;
        
    }




   
    public float DistanceToPool()
    {

        float distance;

        distance = (transform.position - poolPosition.position).magnitude;

        return distance;


    }



}
