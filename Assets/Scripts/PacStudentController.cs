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
                        if (lastInput != currentInput)
                        {
                            activeTweens.Remove(activeTweens[i]);
                        }
                        else
                        {
                            Transform lastTarget = activeTweens[i].Target;
                            activeTweens.Remove(activeTweens[i]);
                            AddTween(lastTarget, currentInput);
                        }
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
    public bool AddTween(Transform targetObject, KeyCode input)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(resolveInput(targetObject, input));
            currentInput = input;
            return true;
        }

        return false;
    }


    public CarModel resolveInput(Transform targetObject, KeyCode input)
    {
        CarModel car;
        switch (input)
        {
            case KeyCode.W:
                car = moveUp(targetObject);
                break;

            case KeyCode.S:
                car = moveDown(targetObject);
                break;

            case KeyCode.A:
                car = moveLeft(targetObject);
                break;

            case KeyCode.D:
                car = moveRight(targetObject);
                break;

            default:
                car = moveRight(targetObject);
                break;
        }

        return car;
    }

    public CarModel moveLeft(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x - Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));        
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, 1f);
    }

    public CarModel moveRight(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x + Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);        
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 270.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, 1f);
    }

    public CarModel moveUp(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y - Constants.yPos_Delta, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, 1f);
    }

    public CarModel moveDown(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y + Constants.yPos_Delta, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, 1f);
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
