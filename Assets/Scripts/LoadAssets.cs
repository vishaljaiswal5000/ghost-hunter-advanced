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
    //private GameObject carInstance;

    public AudioSource backgroundIntro;
    public AudioSource ghostScared;

    public GameObject panelLife;

    // Constants
    const float xPositionDelta = 0.550f;
    const int totalLife = 3;

    public static bool isIntroCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        panelLife = GameObject.FindGameObjectWithTag("Panel-Lives");
        SetupGhost();
        //SetupIndicators();
    }

    // Update is called once per frame
    void Update()
    {
        FitCamera();

        // intro lasts 4 seconds (matches the 3-2-1-Go countdown)
        if (!isIntroCompleted && Time.timeSinceLevelLoad > 4.0f)
        {
            // it will stop music and time property set 0.
            backgroundIntro.Stop();

            // Instantiating car 
            //carInstance = Instantiate(car, car.transform.position, car.transform.rotation);
            Instantiate(car, car.transform.position, car.transform.rotation);
            // Playing car background music
            //backgroundNormal.Play();

            isIntroCompleted = true;
        }

        if (Constants.isGhostScared && !ghostScared.isPlaying)
        {
            ghostScared.Play();
        }

        if (!Constants.isGhostScared)
        {
            ghostScared.Stop();
        }
    }

    // Keep the whole maze (about 9.8 units wide, 10 tall) visible on narrow / portrait screens
    void FitCamera()
    {
        Camera cam = Camera.main;
        if (cam != null && cam.orthographic)
        {
            cam.orthographicSize = Mathf.Max(5f, 4.9f / cam.aspect);
        }
    }

    // Showing Life and bonus indicators in the bottom of the level
    public void SetupIndicators()
    {
        float currY = 0;
        // Create indicators as per defined in the totalLife variable
        for (int i = 0; i < totalLife; i++)
        {
            GameObject life = Instantiate(carLifeIndicator, new Vector3(carLifeIndicator.transform.position.x, carLifeIndicator.transform.position.y - currY, carLifeIndicator.transform.position.z), car.transform.rotation);
            life.name = "Life-" + (i + 1);
            life.transform.parent = panelLife.transform;

            currY += 24.5f;
        }

        // Show Cherry/Nos Bottle in the Bottom-right corner of the level layout
        //Instantiate(bonusPellet, bonusPellet.transform.position, bonusPellet.transform.rotation);
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
