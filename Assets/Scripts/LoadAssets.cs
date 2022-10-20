using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssets : MonoBehaviour
{
    // Public variables
    public GameObject car;
    public GameObject blueGhost;
    public GameObject greenGhost;
    public GameObject redGhost;
    public GameObject yellowGhost;
    public GameObject bonusPellet;
    public GameObject carLifeIndicator;
    public GameObject bonusPelletIndicator;
    private GameObject carInstance;

    public AudioSource backgroundIntro;
    public AudioSource backgroundNormal;

    // Constants
    const float xPositionDelta = 0.550f;
    const int totalLife = 3;

    // Start is called before the first frame update
    void Start()
    {
        SetupGhost();        
        SetupIndicators();        
    }

    // Update is called once per frame
    void Update()
    {
        if (backgroundIntro.time > 5.0f)
        {
            // it will stop music and time property set 0.
            backgroundIntro.Stop();

            // Instantiating car 
            carInstance = Instantiate(car, car.transform.position, car.transform.rotation);

            // Playing car background music
            backgroundNormal.Play();
        }

    }

    // Showing Life and bonus indicators in the bottom of the level
    public void SetupIndicators()
    {
        float currX = 0;
        // Create indicators as per defined in the totalLife variable
        for (int i = 0; i < totalLife; i++)
        {
            Instantiate(carLifeIndicator, new Vector3(carLifeIndicator.transform.position.x + currX, carLifeIndicator.transform.position.y, carLifeIndicator.transform.position.z), car.transform.rotation).name = "Power Indicator " + (i + 1);
            currX += xPositionDelta;
        }

        // Show Cherry/Nos Bottle in the Bottom-right corner of the level layout
        Instantiate(bonusPellet, bonusPellet.transform.position, bonusPellet.transform.rotation);
    }
    
    // Instantiating ghost in the level
    public void SetupGhost()
    {
        Instantiate(greenGhost, greenGhost.transform.position, greenGhost.transform.rotation);
        Instantiate(redGhost, redGhost.transform.position, redGhost.transform.rotation);
        Instantiate(blueGhost, blueGhost.transform.position, blueGhost.transform.rotation);
        Instantiate(yellowGhost, yellowGhost.transform.position, yellowGhost.transform.rotation);
    }
}
