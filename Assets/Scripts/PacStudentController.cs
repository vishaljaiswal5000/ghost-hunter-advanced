using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public CarModel carModel;    
    public float duration = 1.5f;
    public string lastInput;
    private Animator mAnimator;
    public void initMovement(CarModel carModel)
    {
        this.carModel = carModel;
        mAnimator = this.carModel.Target.GetComponent<Animator>();    
    }
        
    void Start()
    {
        
    }

    
    void Update()
    {
        StartCoroutine(StartMovement());
    }

    public float EaseIn(float t)
    {
        return Mathf.Pow(t, 3);
    }

    // Managing car movement
    public IEnumerator StartMovement()
    {
        // For Assigment 3 only - Checking if car is wrecked/dead by animation parameter
        if (carModel!=null && !mAnimator.GetBool("isDead"))
        {
            //between vector A and B
            float distance = Vector3.Distance(carModel.StartPos, carModel.EndPos);            

            float timeFraction = (Time.time - carModel.StartTime) / carModel.Duration;           
            
            // Updating position
            if (timeFraction > 1)
            {
                carModel.Target.position = Vector3.Lerp(carModel.StartPos,carModel.EndPos,timeFraction);                
            }
            else
            {
                carModel.Target.position = Vector3.Lerp(carModel.StartPos, carModel.EndPos, timeFraction);
            }
        }
        yield return null;
    }
}
