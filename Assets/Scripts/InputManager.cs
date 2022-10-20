using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameObject item;
    private Movement movement;

    void Start()
    {
        // Fetching movement component
        this.movement = GetComponent<Movement>();
        item = transform.gameObject;

        // Providing car model for initaiting movement 
        movement.initMovement(new CarModel(item.transform, item.transform.position, item.transform.position, Time.time, 1.5f));
    }

}
