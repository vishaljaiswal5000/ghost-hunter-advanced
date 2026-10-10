using UnityEngine;

// Drives the ghost animator parameters from the ghost's state
public class GhostAnimator : MonoBehaviour
{
    private Animator mAnimator;
    public AudioSource ghostDead;
    public GhostController ghostController;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        ghostController = GetComponent<GhostController>();
        GameObject deadSound = GameObject.FindGameObjectWithTag("Sound-Ghost-Dead");
        if (deadSound != null) ghostDead = deadSound.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (mAnimator == null || ghostController == null) return;

        bool isDead = ghostController.state == GhostState.Dead;
        bool isScared = ghostController.IsScared;

        mAnimator.SetBool("isDead", isDead);
        mAnimator.SetBool("isScared", isScared);
        mAnimator.SetBool("isRecovering", isScared && Constants.isGhostRecovering);
    }
}
