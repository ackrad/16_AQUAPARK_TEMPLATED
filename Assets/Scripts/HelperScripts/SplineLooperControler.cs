using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Dreamteck.Splines;
using NaughtyAttributes;

public class SplineLooperControler : MonoBehaviour
{
    // public GameObject[] prefabs;
    //
    // private SplineComputer path;
    // private List<SplineFollower> spawnedPrefabs;
    //
    // private bool isActive = false;
    //
    // public float spawnPeriod = 1f;
    // public float actorSpeed = 1f;
    //
    // private float carTimer = 0f;
    //
    // public bool reversedForward = false;
    //
    // public Vector2 offsetNoise;
    //
    // public bool fromBothSide = false;
    //
    // void Awake()
    // {
    //     spawnedPrefabs=new List<SplineFollower>();
    // }
    //
    // // Start is called before the first frame update
    // void Start()
    // {
    //     path = GetComponentInChildren<SplineComputer>();
    // }
    //
    // void Update()
    // {
    //     if (isActive)
    //     {
    //         carTimer += Time.deltaTime;
    //         if (carTimer >= spawnPeriod)
    //         {
    //             carTimer %= spawnPeriod;
    //             spawnPrefab(prefabs[Random.Range(0,prefabs.Length)]);
    //             carTimer = 0;
    //         }
    //     }
    // }
    //
    // private void spawnPrefab(GameObject prefab)
    // {
    //     GameObject spawnedPrefab = Instantiate(prefab, transform);
    //     spawnedPrefab.transform.position=new Vector3(0,10000,0);
    //
    //     if (reversedForward)
    //     {
    //         GameObject spawnedPrefabParent = new GameObject();
    //         spawnedPrefabParent.transform.parent = transform;
    //         spawnedPrefabParent.transform.position = spawnedPrefab.transform.position;
    //         spawnedPrefab.transform.parent = spawnedPrefabParent.transform;
    //         spawnedPrefab.transform.localRotation = Quaternion.Euler(0, 180, 0);
    //         spawnedPrefab = spawnedPrefabParent;
    //     }
    //     
    //     SplineFollower splineFollower = spawnedPrefab.AddComponent<SplineFollower>();
    //     splineFollower.spline = path;
    //     splineFollower.follow = true;
    //     splineFollower.followSpeed = actorSpeed;
    //
    //
    //     bool isReversed = (fromBothSide && Random.value >= 0.5f);
    //
    //     if (isReversed)
    //     {
    //         splineFollower.direction = Spline.Direction.Backward;
    //         splineFollower.startPosition = 1f;
    //
    //     }
    //
    //
    //     splineFollower.motion.offset = new Vector2(Random.Range(0, offsetNoise.x), Random.Range(0, offsetNoise.y));
    //
    //     if (isReversed)
    //     {
    //         splineFollower.onBeginningReached += (double d) =>
    //         {
    //             spawnedPrefabs.Remove(splineFollower);
    //             Destroy(spawnedPrefab);
    //         };
    //     }
    //     else
    //     {
    //         splineFollower.onEndReached += (double d) =>
    //         {
    //             spawnedPrefabs.Remove(splineFollower);
    //             Destroy(spawnedPrefab);
    //         };
    //     }
    //     
    //     spawnedPrefabs.Add(splineFollower);
    //     
    //     spawnedPrefab.name = "LooperActor";
    //     
    //     spawnedPrefab.AddComponent<BoxCollider>();
    //     Rigidbody body=spawnedPrefab.AddComponent<Rigidbody>();
    //     body.isKinematic = true;
    // }
    //
    // public void breakLoop(GameObject prefab, float Force)
    // {
    //     SplineFollower follower = prefab.GetComponent<SplineFollower>();
    //     if(follower==null || spawnedPrefabs.Contains(follower)==false) return;
    //
    //     Vector3 randomDirection = Random.insideUnitSphere;
    //
    //     stopActors();
    //     
    //     Rigidbody body = follower.GetComponent<Rigidbody>();
    //     body.isKinematic = false;
    //     body.AddForce(Force*randomDirection);
    // }
    //
    // public void stopActors()
    // {
    //     foreach (SplineFollower splineFollower in spawnedPrefabs)
    //     {
    //         splineFollower.followSpeed = 0;
    //     }
    //     
    // }
    //
    // [Button("Activate")]
    // public void Activate()
    // {
    //     setActive(true);
    // }
    //
    // public void setActive(bool activate)
    // {
    //     isActive = activate;
    //
    //     if (!activate)
    //     {
    //         clearActors();
    //     }
    // }
    //
    // public void clearActors()
    // {
    //     foreach (SplineFollower splineFollower in spawnedPrefabs)
    //     {
    //         Destroy(splineFollower.gameObject);
    //     }
    //     spawnedPrefabs.Clear();
    // }
    

}
