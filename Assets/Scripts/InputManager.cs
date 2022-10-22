using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject car;
    private void Start()
    {

    }

    private void Update()
    {
        if (car == null)
        {
            car = GameObject.FindGameObjectWithTag("Car");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            CarModel carModel = new CarModel(car.transform, car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y + (0.639f), car.transform.position.z), Time.time, 1f);
            car.GetComponent<PacStudentController>().initMovement(carModel);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            CarModel carModel = new CarModel(car.transform, car.transform.position, new Vector3(car.transform.position.x, car.transform.position.y + (-0.639f), car.transform.position.z), Time.time, 1f);
            car.GetComponent<PacStudentController>().initMovement(carModel);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CarModel carModel = new CarModel(car.transform, car.transform.position, new Vector3(car.transform.position.x + (-0.6000002f), car.transform.position.y , car.transform.position.z), Time.time, 1f);
            car.GetComponent<PacStudentController>().initMovement(carModel);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CarModel carModel = new CarModel(car.transform, car.transform.position, new Vector3(car.transform.position.x + (0.6000002f), car.transform.position.y, car.transform.position.z), Time.time, 1f);
            car.GetComponent<PacStudentController>().initMovement(carModel);
        }
    }
}
