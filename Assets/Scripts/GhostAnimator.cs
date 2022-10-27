using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimator: MonoBehaviour
{
    // Private variables
    private Animator mAnimator;    
    private bool isDeadPlayed = false;
    private bool isScaredPlayed = false;


    void Start()
    {        
        mAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        if (mAnimator != null)
        {
            if (mAnimator.GetBool("isScared") && Constants.isGhostRecovering)
            {
                mAnimator.SetBool("isRecovering", true);
            }
            else if (Constants.isGhostScared)
            {
                mAnimator.SetBool("isScared",true);
            }
            else
            {
                mAnimator.SetBool("isScared", false);
                mAnimator.SetBool("isRecovering", false);
            }
        }
    }
}
