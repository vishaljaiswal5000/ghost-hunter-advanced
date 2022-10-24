using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public CarModel carModel;
    public KeyCode lastInput;
    public KeyCode currentInput;
    private Animator mAnimator;
    private AudioSource mAudioSource;
    private List<CarModel> activeTweens = new List<CarModel>();

    public int[] currPosition = Constants.levelMapStartPos;
    public void initMovement(CarModel carModel)
    {
        this.carModel = carModel;
        mAnimator = this.carModel.Target.GetComponent<Animator>();
    }

    void Start()
    {
        mAudioSource = gameObject.GetComponent<AudioSource>();
        mAudioSource.Stop();
        mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.enabled = false;
    }


    void Update()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            if (activeTweens[i] != null)
            {
                currentInput = lastInput;
                if (!mAnimator.GetBool("isDead"))
                {
                    mAnimator.enabled = true;
                    if (!mAudioSource.isPlaying)
                    {
                        mAudioSource.Play();
                    }
                    //between vector A and B
                    float distance = Vector3.Distance(activeTweens[i].StartPos, activeTweens[i].EndPos);

                    float timeFraction = (Time.time - activeTweens[i].StartTime) / activeTweens[i].Duration;

                    // Updating position
                    if (timeFraction > 1)
                    {
                        activeTweens.Remove(activeTweens[i]);
                        mAnimator.enabled = false;
                        mAudioSource.Stop();
                    }
                    else
                    {
                        activeTweens[i].Target.position = Vector3.Lerp(activeTweens[i].StartPos, activeTweens[i].EndPos, timeFraction);

                    }
                }
                else
                {
                    mAudioSource.Stop();
                    mAnimator.enabled = false;
                }
            }
        }
    }

    public float EaseIn(float t)
    {
        return Mathf.Pow(t, 3);
    }

    // Managing car movement
    public IEnumerator StartMovement()
    {
        // For Assigment 3 only - Checking if car is wrecked/dead by animation parameter
        if (carModel != null && !mAnimator.GetBool("isDead"))
        {
            //between vector A and B
            float distance = Vector3.Distance(carModel.StartPos, carModel.EndPos);

            float timeFraction = (Time.time - carModel.StartTime) / carModel.Duration;

            // Updating position
            if (timeFraction > 1)
            {
                carModel.Target.position = Vector3.Lerp(carModel.StartPos, carModel.EndPos, timeFraction);
            }
            else
            {
                carModel.Target.position = Vector3.Lerp(carModel.StartPos, carModel.EndPos, timeFraction);
            }
        }
        yield return null;
    }


    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float Duration, KeyCode input)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new CarModel(targetObject, startPos, endPos, Time.time, Duration));
            lastInput = input;
            return true;
        }

        return false;
    }

    public bool TweenExists(Transform target)
    {
        // Linq instead of loop
        CarModel tween = activeTweens.Find(x => x.Target == target);

        if (tween != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
