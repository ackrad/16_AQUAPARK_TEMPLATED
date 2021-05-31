using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class HapticFeedbackController : Singleton<HapticFeedbackController>
{
    
    [Range(0,1)] public float continuesHapticIntensity = 1f;
    [Range(0, 1)] public float continuesHapticSharpness = 0.5f;

    public float minimumDurationBetweenHaptics = 0.05f;
    private float lastHapticTime = 0f;

    private float continousHapticMinTime = 0.25f;
    private float lastOrderTime = 0f;
    public bool isContinousHapticRunning = false;

    public bool Mute = false;
    
    public void VibrateSuccess(){
        Vibrate(HapticTypes.Success);
    }

    public void VibrateFailure(){
        Vibrate(HapticTypes.Failure);
    }
    
    public void VibrateLight(){
        Vibrate(HapticTypes.LightImpact);
    }
    
    public void VibrateSoft(){
        Vibrate(HapticTypes.SoftImpact);
    }
    
    public void VibrateHeavy(){
        Vibrate(HapticTypes.HeavyImpact);
    }

    public void Vibrate(HapticTypes hapticType)
    {
        if(Mute) return;
        if(minimumDurationBetweenHaptics>0 && Time.time-lastHapticTime<minimumDurationBetweenHaptics) return;
        MMVibrationManager.Haptic(hapticType);
        lastHapticTime = Time.time;
    }

    void Update()
    {
        if (isContinousHapticRunning)
        {
            if (Time.time - lastOrderTime > continousHapticMinTime)
            {
                StopContinuesHaptic();
            }
        }
    }

    public void ThrowContinuesHaptic()
    {
        if(Mute) return;
        lastOrderTime = Time.time;
        if(isContinousHapticRunning) return;
        MMVibrationManager.ContinuousHaptic(continuesHapticIntensity,continuesHapticSharpness,1000f,HapticTypes.LightImpact);
        isContinousHapticRunning = true;


    }

    public void StopContinuesHaptic()
    {
        MMVibrationManager.StopContinuousHaptic();
        isContinousHapticRunning = false;
    }



}
