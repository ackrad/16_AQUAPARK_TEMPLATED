using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sound Controller controls sound effects in the game
public class SoundController : MonoBehaviour
{

    public static SoundController instance;

    //If is true same clip can be spawned at the same time, if false same clip can not be played 
    public bool enableDuplicateClips = false;

    public bool isOn { get; private set; } = true;

    private void Awake()
    {
        if (instance != null)        
            Destroy(this);        
        else        
            instance = this;
        
    }

    public void createSoundEffect(AudioClip clip)
    {
        if (isOn == false)
            return;

        //If duplicate is not allowed and provided clip is playing dont create sound effect
        if (enableDuplicateClips == false && isClipPlaying(clip))
            return;


        //Create audio source for the clip
        var source = gameObject.AddComponent<AudioSource>();

        //Set clip to source and play it immidieatly
        source.clip = clip;
        source.Play();

        //Destroy clip when it is finished
        Destroy(source, clip.length + 1);
        
    }

    //Checks weather or not there is audio source playing provided clip
    public bool isClipPlaying(AudioClip clip)
    {
        AudioSource[] currentSources = GetComponents<AudioSource>();
        foreach(var source in currentSources)
        {
            if (source.clip == clip)
            {
                return true;
            }
        }

        return false;

    }

    //Use this function to get singleton class instantiation
    public static SoundController request()
    {
        if (SoundController.instance == null)
        {
            Debug.LogError("There is no sound controller in scene");
        }

        return SoundController.instance;
    }
    
    //Use this function to mute sound effects
    //If muted is true, it will muted else it will unmuted
    public void switchSound(bool on)
    {
        isOn = on;
    }

}
