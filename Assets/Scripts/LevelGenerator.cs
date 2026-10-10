using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // public variables
    public GameObject wall_1;
    public GameObject wall_2;
    public GameObject wall_3;
    public GameObject wall_4;
    public GameObject wall_7;

    public GameObject powerPellet;
    public GameObject normalPellet;

    // private variables
    private GameObject quadrant_1;
    private GameObject quadrant_2;
    private GameObject quadrant_3;
    private GameObject quadrant_4;
    private GameObject upperQuad;
    private GameObject lowerQuad;
    private GameObject layout;


    // Transform presets
    static private Vector3 quad1_pos = new Vector3(0.94f, 2.34f, 0.0f);
    static private Vector3 quad2_pos = new Vector3(-0.94f, 2.34f, 0.0f);

    private Vector3 quad3_pos = quad1_pos;
    private Vector3 quad4_pos = quad2_pos;

    private Vector3 Rotation_0 = new Vector3(0, 0, 0);
    private Vector3 xRotation_180 = new Vector3(180, 0, 0);
    private Vector3 yRotation_180 = new Vector3(0, 180, 0);
    private Vector3 zRotation_180 = new Vector3(0, 0, 180);
    private Vector3 zRotation_270 = new Vector3(0, 0, 270);
    private Vector3 zRotation_90 = new Vector3(0, 0, 90);

    private Vector3 quadScale = new Vector3(0.5f, 0.5f, 1f);

    // Constants
    const float initial_xPos = -10.0f;
    const float initial_yPos = 4.5f;
    const float increment_xPos = 0.6000002f;
    const float increment_yPos = -0.639f;

    float currX = initial_xPos;
    float currY = initial_yPos;

    // for wall rotations 
    bool wall1_rotationCounter, wall3_rotationCounter = true;
     

    void Start()
    {
        // Delete manual level layout
        Destroy(GameObject.FindGameObjectWithTag("Layout"));

        // Creating empty game object (parent)
        layout = new GameObject("Layout");
        upperQuad = new GameObject("Upper_quad");
        lowerQuad = new GameObject("Lower_quad");
        quadrant_1 = new GameObject("Quad_1");

        Setup();
        PostSetup();
    }

    // Instantiating game objects and assigning in Gameobject (quadrant 1)
    public void InstantiateGameObject(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = Instantiate(gameObject, position, rotation);
        newObject.transform.parent = quadrant_1.transform;
    }

    // After Creating wall
    // Assigning parent and creating clone quadrants
    public void PostSetup()
    {
        // Setting quadrant 1 transform (position and scale)
        quadrant_1.transform.localScale = quadScale;
        quadrant_1.transform.position = quad1_pos;

        // Creating quadrant 2 (from quadrant 1)
        quadrant_2 = Instantiate(quadrant_1, quad2_pos, Quaternion.Euler(yRotation_180));
        quadrant_2.name = "Quad_2";

        // Creating quadrant 3 (from quadrant 1)
        quadrant_3 = Instantiate(quadrant_1, quad3_pos, Quaternion.Euler(Rotation_0));
        quadrant_3.name = "Quad_3";

        // Creating quadrant 4 (from quadrant 1)
        quadrant_4 = Instantiate(quadrant_1, quad4_pos, Quaternion.Euler(yRotation_180));
        quadrant_4.name = "Quad_4";

        // Assigning Upper Quad as parent to Quad1 and Quad2
        quadrant_1.transform.parent = upperQuad.transform;
        quadrant_2.transform.parent = upperQuad.transform;

        // Assigning Lower Quad as parent to Quad3 and Quad4
        quadrant_3.transform.parent = lowerQuad.transform;
        quadrant_4.transform.parent = lowerQuad.transform;

        // Setting lower quad transform
        lowerQuad.transform.rotation = Quaternion.Euler(xRotation_180);

        // Assigning Layout as Parent to both upper and lower quad
        upperQuad.transform.parent = layout.transform;
        lowerQuad.transform.parent = layout.transform;

        // Layout (All quadrants) pushing one step towards the Z-axis, so Car (0,0,0) will looks like moving above the pellets.
        layout.transform.position = new Vector3(layout.transform.position.x, layout.transform.position.y, 1.0f);
    }

    // Create level programmatically
    public void Setup()
    {
        for (int i = 0; i < 15; i++)
        {
            wall1_rotationCounter = true;
            wall3_rotationCounter = true;

            for (int j = 0; j < 14; j++)
            {
                switch (Constants.levelMap[i, j])
                {
                    case 1:
                        BuildWall1(i, j, wall_1);
                        break;

                    case 2:
                        BuildWall2(i, j, wall_2);
                        break;

                    case 3:
                        BuildWall3(i, j, wall_3);
                        break;

                    case 4:
                        BuildWall4(i, j, wall_4);
                        break;

                    case 5:
                        InstantiateGameObject(normalPellet, new Vector3(currX, currY, 0), Quaternion.identity);
                        break;

                    case 6:
                        InstantiateGameObject(powerPellet, new Vector3(currX, currY, 0), Quaternion.identity);
                        break;

                    case 7:
                        InstantiateGameObject(wall_7, new Vector3(currX, currY, 0), Quaternion.identity);
                        break;
                }
                currX += increment_xPos;
            }

            // re-initialing to starting X-position
            currX = initial_xPos;

            // Adding Y-position delta
            currY += increment_yPos;
        }
    }

    // For Wall 1 design
    public void BuildWall1(int row, int col, GameObject wall)
    {
        if (row == 0)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.identity);
        }
        else if (row == 9 && wall1_rotationCounter)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_90));
            wall1_rotationCounter = !wall1_rotationCounter;
        }
        else if (row == 9 && !wall1_rotationCounter)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_270));
        }
        else if (row == 13)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_180));
        }
    }

    // For Wall 2 design
    public void BuildWall2(int row, int col, GameObject wall)
    {
        if (row == 0 || row == 14 || row == 9 || row == 13)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.identity);
        }
        else if (col == 0 || col == 5)
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_90));
        }
    }

    // For Wall 3 design
    public void BuildWall3(int row, int col, GameObject wall)
    {
        Vector3 rotation;
        rotation = new Vector3(0, 0, 0);
        if (row == 7 && col == 13)
        {
            rotation = zRotation_270;
        }
        else if (row == 10 && col == 13)
        {
            rotation = zRotation_90;
        }
        else if ((row == 2 || row == 6 || row == 10 || row == 12) && wall3_rotationCounter)
        {
            rotation = new Vector3(0, 0, 0);
        }
        else if ((row == 2 || row == 6 || row == 9 || row == 12) && !wall3_rotationCounter)
        {
            rotation = zRotation_270;
        }
        else if ((row == 4 || row == 7 || row == 9 || row == 13) && wall3_rotationCounter)
        {
            rotation = zRotation_90;
        }
        else if ((row == 4 || row == 7 || row == 10 || row == 13) && !wall3_rotationCounter)
        {
            rotation = zRotation_180;
        }

        InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(rotation));
        wall3_rotationCounter = !wall3_rotationCounter;
    }

    // For Wall 4 design
    public void BuildWall4(int row, int col, GameObject wall)
    {
        if (row == 1 || row == 3 || row == 8 || row == 11 || row == 13 || row == 14)
        {
            InstantiateGameObject(wall_4, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_90));
        }
        else if (row == 9 || row == 10 || row == 12)
        {
            if (col == 9 || col == 10 || col == 11 || col == 12)
            {
                InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(Rotation_0));
            }
            else
            {
                InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_90));
            }
        }
        else if ((row == 2 && col == 13) || (row == 7 && (col == 7 || col == 8)))
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(zRotation_90));
        }
        else
        {
            InstantiateGameObject(wall, new Vector3(currX, currY, 0), Quaternion.Euler(Rotation_0));
        }
    }
}
