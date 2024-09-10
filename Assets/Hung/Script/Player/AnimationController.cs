using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
        
    void Update()
    {
        bool pressW = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        if (pressW)
        {
            animator.SetBool("isRunning", true);
        }
        if (!pressW)
        {
            animator.SetBool("isRunning", false);
            animator.SetFloat("Velocity", 0f);
        }

        if(runPressed && pressW)
        {
            animator.SetFloat("Velocity", 1f);
        }

        if(!runPressed && pressW)
        {
            Debug.Log("walk");
            animator.SetFloat("Velocity", 0.1f);
        }
    }
}
