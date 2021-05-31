using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  

public class FB_Manager : MonoBehaviour
{

     public static FB_Manager instance;

     private void Awake()
     {
        
         if (instance == null)
         {
             instance = this;
             DontDestroyOnLoad(gameObject);
             Init();
         }
         else
             Destroy(gameObject);
     }


     void Init()
     {
         if (!Facebook.Unity.FB.IsInitialized)
         {
             // Initialize the Facebook SDK
             Facebook.Unity.FB.Init(InitCallback, OnHideUnity);
         
         }
         else
         {
             // Already initialized, signal an app activation App Event
             Facebook.Unity.FB.ActivateApp();
         }

     }

     private void InitCallback()
     {
         if (Facebook.Unity.FB.IsInitialized)
         {
             // Signal an app activation App Event
             Facebook.Unity.FB.ActivateApp();
             //SaveLoadManager.IncreaseLTAppOpen();
             // Continue with Facebook SDK
             // ...
         }
         else
         {
             Debug.Log("Failed to Initialize the Facebook SDK");
         }
     }

     private void OnHideUnity(bool isGameShown)
     {
         if (!isGameShown)
         {
             // Pause the game - we will need to hide
             Time.timeScale = 0;
         }
         else
         {
             // Resume the game - we're getting focus again
             Time.timeScale = 1;
         }
     }

     public static void sendFacebookEvent(string header, string label)
     {

         if (!Facebook.Unity.FB.IsInitialized)
         {
             instance.Init();
             return;
         }
         var tutParams = new Dictionary<string, object>();
         tutParams["header"] = header;
         tutParams[label] = "label";

         Facebook.Unity.FB.LogAppEvent(
             header,
             parameters: tutParams
         );
     }

}
