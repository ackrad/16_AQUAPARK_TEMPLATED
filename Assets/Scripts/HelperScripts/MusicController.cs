using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    //Singleton class
    public static MusicController instance;

    [System.Serializable]
    public struct SceneMusic
    {
        public int SceneIndex;
        public AudioClip MusicClip;
    }

    public SceneMusic[] SceneMusics;

    public bool isOn { get; private set; } = true;

    AudioSource musicSource;

    private void Awake()
    {
        if (instance != null)
            Destroy(this);
        else
            instance = this;

    }


    // Start is called before the first frame update
    void Start()
    {
        //First create audio source
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.mute = !isOn;

        //Music source must be looped
        musicSource.loop = true;

        //Find scene music with scene index
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        foreach(var sceneMusic in SceneMusics)
        {
            if (sceneMusic.SceneIndex == currentSceneIndex)
            {
                musicSource.clip = sceneMusic.MusicClip;
            }
        }

        //If couldnt find any clip throw error
        if (musicSource.clip == null)
        {
            Debug.LogError("Couldn't find music clip for this scene index: "+currentSceneIndex);
            Destroy(this);
            return;
        }
        else
        {
            musicSource.Play();

        }

    }

    //Use this function to mute sound effects
    //If muted is true, it will muted else it will unmuted
    public void switchSound(bool on)
    {
        isOn = on;
        musicSource.mute = !isOn;

    }


    public static MusicController request()
    {
        if (MusicController.instance == null)
        {
            Debug.LogError("There is no music controller in scene");
        }

        return MusicController.instance;
    }

}
