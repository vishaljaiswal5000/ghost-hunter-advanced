using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public GameObject car;
    public PacStudentController pacStudentController;
    private void Start()
    {
       
    }

    private void Update()
    {
        if (car == null && LoadAssets.isIntroCompleted && SceneManager.GetActiveScene().buildIndex == 1)
        {
            car = GameObject.FindGameObjectWithTag("Car");
            this.pacStudentController = car.GetComponent<PacStudentController>();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            pacStudentController.AddTween(car.transform, car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y - Constants.yPos_Delta, car.transform.position.z), 1f, KeyCode.W);

        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            pacStudentController.AddTween(car.transform, car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y + Constants.yPos_Delta, car.transform.position.z), 1f, KeyCode.S);

        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            pacStudentController.AddTween(car.transform, car.transform.position, new Vector3(car.transform.position.x - Constants.xPos_Delta, car.transform.position.y, car.transform.position.z), 1f, KeyCode.A);

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            pacStudentController.AddTween(car.transform, car.transform.position, new Vector3(car.transform.position.x + Constants.xPos_Delta, car.transform.position.y, car.transform.position.z), 1f, KeyCode.D);

        }
    }
}
