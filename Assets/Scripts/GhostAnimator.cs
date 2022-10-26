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
            if (mAnimator.GetBool("isScared") && !isScaredPlayed)
            {
                // play audio
                isScaredPlayed = true;
            }
            else if (mAnimator.GetBool("isScared") && mAnimator.GetBool("isDead") && !isDeadPlayed)
            {
                // play audio
                isDeadPlayed = true;
            }
            else
            {
                isScaredPlayed = false;
                isDeadPlayed = false;
            }
        }
    }
}
