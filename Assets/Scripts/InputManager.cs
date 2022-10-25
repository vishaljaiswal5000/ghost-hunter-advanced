using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public GameObject car;
    public PacStudentController pacStudentController;
    
    private void Update()
    {
        if (car == null && LoadAssets.isIntroCompleted && SceneManager.GetActiveScene().buildIndex == 1)
        {
            car = GameObject.FindGameObjectWithTag("Car");
            this.pacStudentController = car.GetComponent<PacStudentController>();
        }

        if (car != null)
        {

            if (Input.GetKeyDown(KeyCode.W))
            {                
                pacStudentController.AddTween(car.transform, KeyCode.W);
                pacStudentController.lastInput = KeyCode.W;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {             
                pacStudentController.AddTween(car.transform, KeyCode.S);
                pacStudentController.lastInput = KeyCode.S;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {             
                pacStudentController.AddTween(car.transform, KeyCode.A);
                pacStudentController.lastInput = KeyCode.A;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {             
                pacStudentController.AddTween(car.transform, KeyCode.D);
                pacStudentController.lastInput = KeyCode.D;
            }
        }
    }
}
