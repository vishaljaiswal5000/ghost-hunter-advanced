using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimator: MonoBehaviour
{
    // Private variables
    private Animator mAnimator;    
    public bool isDead = false;
    private bool isScaredPlayed = false;
    public AudioSource ghostDead;
    private float timer =0f;

    void Start()
    {        
        mAnimator = GetComponent<Animator>();
        ghostDead = GameObject.FindGameObjectWithTag("Sound-Ghost-Dead").GetComponent<AudioSource>();
    }


    void Update()
    {
        if (mAnimator != null)
        {
            if (isDead)
            {
                timer += Time.deltaTime;
                mAnimator.SetBool("isDead", true);
                if (timer > 5)
                {
                    isDead = false;
                    timer = 0f;
                    mAnimator.SetBool("isDead", false);
                }
            }
            else if (mAnimator.GetBool("isScared") && Constants.isGhostRecovering)
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
