using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimator : MonoBehaviour
{

    private Animator mAnimator;
    [SerializeField] private AudioSource dead;
    [SerializeField] private AudioSource collide;
    [SerializeField] private AudioSource normalPelletEaten;
    [SerializeField] private AudioSource powerPelletEaten;
    [SerializeField] private AudioSource bonusPelletEaten;

    private bool isDead = false;
    private bool isAudioPlayed = false;
    GameObject car;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(getCar());
    }

    // Update is called once per frame
    void Update()
    {
        if (mAnimator != null)
        {
            // For assignment 3 - checking
            isDead = mAnimator.GetBool("isDead");
            if (isDead && !isAudioPlayed)
            {
                dead.Play();
                isAudioPlayed = !isAudioPlayed;
            }

         /*   mAnimator.SetBool("isDead", isDead)*/;
        }
    }

    // Finding Gameobject Car and Fetching its animator
    IEnumerator getCar()
    {
        car = GameObject.Find("Car(Clone)");
        if (car)
        {
            mAnimator = car.GetComponent<Animator>();
        }

        yield return null;
    }
}
