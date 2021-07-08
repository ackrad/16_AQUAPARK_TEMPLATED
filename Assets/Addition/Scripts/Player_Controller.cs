using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    //cached variables
    Avatar_Controller avatar_Controller;

    //for leantouch
    Vector2 fingerPos = new Vector2(0f, 0f);
    [SerializeField] float inputLeeway = 20f;

    private void OnEnable()
    {
        Lean.Touch.LeanTouch.OnFingerDown += GetContactPoint;
        Lean.Touch.LeanTouch.OnFingerUp += FingerIsUp;


    }

    private void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerDown -= GetContactPoint;
        Lean.Touch.LeanTouch.OnFingerUp -= FingerIsUp;

    }

    void Start()
    {
        avatar_Controller = GetComponent<Avatar_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        var fingers = Lean.Touch.LeanTouch.Fingers;

        if (fingers.Count < 1) { return; }

        Lean.Touch.LeanFinger finger = fingers[0];

        if (finger.ScreenPosition.x > fingerPos.x + inputLeeway)
        {

            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.right;
        }

        else if (finger.ScreenPosition.x < fingerPos.x - inputLeeway)
        {
            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.left;

        }

        else
        {
            avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.middle;
        }

    }

    private void GetContactPoint(Lean.Touch.LeanFinger finger)
    {
        fingerPos = finger.LastScreenPosition;


    }


    private void FingerIsUp(Lean.Touch.LeanFinger finger)
    {

        avatar_Controller.player_Movement = Avatar_Controller.MovementEnum.middle;


    }
}
