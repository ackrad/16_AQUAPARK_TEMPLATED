using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    const string fall = "fall";
    const string victory = "Victory";
    const string slide = "slide";



    public void Fall()
    {


        animator.SetBool("isSliding", false);
        animator.ResetTrigger(victory);

    }

    public void Slide()
    {

        animator.SetBool("isSliding", true);
        animator.ResetTrigger(victory);

    }

    public void Victory()
    {

        animator.SetTrigger(victory);
    }
}
