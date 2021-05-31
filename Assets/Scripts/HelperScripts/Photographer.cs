using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public struct Resolution
{
    public string name;
    public int width;
    public int height;
}

public class Photographer : MonoBehaviour
{
    public bool breakAfterShot = true;
    public KeyCode keyCode=KeyCode.S;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCode)){
            // Screen.SetResolution()
            TakeScreenShot();
        }
    }
    [Button("Take Screen Shot")]
    public void TakeScreenShot()
    {
        RectTransform mainCanvas=UIManager.request().mainCanvas.GetComponent<RectTransform>();
        
        float width=mainCanvas.rect.width;
        float height = mainCanvas.rect.height;
        
        string name = Application.persistentDataPath + "/" + width + "_" + height+"_"+Time.time;
        Debug.Log("Took screen shot and saved to: "+name);
        
        ScreenCapture.CaptureScreenshot(name+".png");
        if(breakAfterShot) Debug.Break();
    }
}