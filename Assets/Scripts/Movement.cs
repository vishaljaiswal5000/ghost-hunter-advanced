using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CarModel carModel;    
    public Vector3 startPos1;
    public Vector3 endPos1;

    public Vector3 startPos2;
    public Vector3 endPos2;

    public Vector3 startPos3;
    public Vector3 endPos3;

    public Vector3 startPos4;
    public Vector3 endPos4;

    Vector3 start;
    Vector3 end;
    float startTime;
    public float duration = 0.5f;
    private Animator mAnimator;
    
    void Start()
    {
        carModel = new CarModel(transform, transform.position, transform.position, Time.time, 1.5f, KeyCode.Return);        
        init();
    }


    void Update()
    {
        StartCoroutine(StartMovement());
    }

    // For ease effect - not using currently
    public float EaseIn(float t)
    {
        return Mathf.Pow(t, 3);
    }

    // Managing car movement
    public IEnumerator StartMovement()
    {
        // For Assigment 3 only - Checking if car is wrecked/dead by animation parameter
        if (carModel!=null)
        {
            //between vector A and B
            float distance = Vector3.Distance(start, end);
            startTime += Time.deltaTime;
            // Distance already moved 
            float completed = startTime / duration;

            // Updating position
            if (completed > 1)
            {
                if (start == startPos1)
                {
                    carModel.Target.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 270.0f));
                    start = startPos2;
                    end = endPos2;
                }
                else if (start == startPos2)
                {
                    carModel.Target.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
                    start = startPos3;
                    end = endPos3;
                }
                else if (start == startPos3)
                {
                    carModel.Target.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));
                    start = startPos4;
                    end = endPos4;
                }
                else
                {
                    carModel.Target.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
                    start = startPos1;
                    end = endPos1;
                }

                startTime = 0.0f;
            }
            else
            {
                carModel.Target.position = Vector3.Lerp(start, end, completed);

            }
        }
        yield return null;
    }

    // initializing position variables 
    public void init()
    {
        startPos1 = new Vector3(200f, 100f, 0.0f);
        endPos1 = new Vector3(200f, 500f, 0.0f);

        startPos2 = endPos1;
        endPos2 = new Vector3(900f, 500f, 0.0f);

        startPos3 = endPos2;
        endPos3 = new Vector3(900f, 100f, 0.0f);

        startPos4 = endPos3;
        endPos4 = startPos1;

        transform.position = startPos1;
        transform.rotation = Quaternion.identity;

        startTime = 1.5f;
        start = startPos1;
        end = endPos1;
    }

}
