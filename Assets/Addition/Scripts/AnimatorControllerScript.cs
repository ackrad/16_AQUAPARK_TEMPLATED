using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControllerScript : MonoBehaviour
{
    [SerializeField] Animator animator;
    const string fall = "IsFalling";
    const string victory = "Victory";
    const string slide = "isSliding";



    public void Fall()
    {

        animator.SetBool(fall, true);
        animator.SetBool(slide, false);
        animator.SetBool(victory, false);

    }

    public void Slide()
    {

        animator.SetBool(slide, true);
        animator.SetBool(victory, false);
        animator.SetBool(fall, false);
    }

    public void Victory()
    {

        animator.SetBool(victory, true);
        animator.SetBool(fall, false);

        animator.SetBool(slide, false);

    }
}
