using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script only work if cinemachine imported
using Cinemachine;

public enum CameraStatus{
    Camera1, Camera2, None
}

[System.Serializable]
public struct VirtualCameras{ 
    public CameraStatus status;
    public CinemachineVirtualCamera camera;
}

public class CameraController : MonoBehaviour
{
    public VirtualCameras[] virtualCameras;
    
    private CinemachineBrain brain;

    CameraStatus currentStatus;

    const int activeValue=10;
    const int passiveValue=1;


    // Start is called before the first frame update
    void Start()
    {
        currentStatus=CameraStatus.Camera1;
        setCameraStatus(currentStatus);
        
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }

    public CameraStatus GetActiveCamStatus(){
        foreach(VirtualCameras vc in virtualCameras){
            if(vc.camera.Priority==activeValue){
                return vc.status;
            }
        }

        return CameraStatus.None;
    }

    public void setCameraStatus(CameraStatus status)
    {
        Dictionary<CinemachineVirtualCamera, int> pritorities=new Dictionary<CinemachineVirtualCamera, int>();
        foreach(VirtualCameras vc in virtualCameras){
            if(vc.status==status){
                pritorities[vc.camera]=activeValue;
            }else{
                pritorities[vc.camera]=passiveValue;
            }
        }
        foreach(CinemachineVirtualCamera vc in pritorities.Keys){
            vc.Priority=pritorities[vc];
        }
        currentStatus=status;

    }
    

    
    public static bool IsPositionInCameraField(Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        if (screenPos.x < Screen.width && screenPos.x > 0 && screenPos.y < Screen.height && screenPos.y > 0)
            return true;
        return false;
    }

}
