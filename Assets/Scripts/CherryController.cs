using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{

    public GameObject bonusPellet;
    public Vector3 startPos, endPos;
    private float startTime;
    private float duration = 10.0f;
    private int axisIndex;
    private int direction;
    private bool isCreated = false;

    void Start()
    {
        bonusPellet = gameObject;
        startPos = getRandomPosition();
        endPos = getEndPos();
    }

    void Update()
    {
        if (bonusPellet != null && startPos != Vector3.zero & endPos != Vector3.zero)
        {
            float distance = Vector3.Distance(startPos, endPos);
            startTime += Time.deltaTime;
            float completed = startTime / duration;

            if (completed > 1)
            {
                Destroy(bonusPellet);
                isCreated = false;
                return;
            }
            else
            {
                bonusPellet.transform.position = Vector3.Lerp(startPos, endPos, completed);

            }
        }
    }

    public Vector3 getRandomPosition()
    {
        // 1 = X and 2 = Y
        axisIndex = Random.Range(1, 3);
        // 1 = normal and 2 = reverse
        direction = Random.Range(1, 3);

        if (!isCreated)
        {
            // random position from X axis
            if (axisIndex == 1)
            {
                startPos = setupPositionX(direction);
                //bonusPellet = Instantiate(bonusPellet, startPos, bonusPellet.transform.rotation);
                
            }
            // random position from Y axis
            else
            {
                startPos = setupPositionY(direction);
                //bonusPellet = Instantiate(bonusPellet, startPos, bonusPellet.transform.rotation);
            }

            bonusPellet.transform.position = startPos;
            isCreated = true;
            return startPos;
        }
        return Vector3.zero;
    }

    public Vector3 setupPositionX(float direction)
    {
        float randomXPos;
        float randomYPos;

        if (direction == 1)
        {
            randomYPos = Constants.minYPos;
        }
        else
        {
            randomYPos = Constants.maxYPos;
        }

        randomXPos = Random.Range(Constants.minXPos, Constants.maxXPos);
        return new Vector3(randomXPos, randomYPos, 0f);
    }

    public Vector3 setupPositionY(float direction)
    {
        float randomXPos;
        float randomYPos;

        if (direction == 1)
        {
            randomXPos = Constants.minXPos;
        }
        else
        {
            randomXPos = Constants.maxXPos;
        }

        randomYPos = Random.Range(Constants.minYPos, Constants.maxYPos);
        return new Vector3(randomXPos, randomYPos, 0f);
    }

    public Vector3 getEndPos()
    {
        if (axisIndex == 1)
        {
            if (direction == 1)
            {
                endPos.y = Constants.maxYPos;
            }
            else
            {
                endPos.y = Constants.minYPos;
            }
            endPos.x = -startPos.x;
        }
        else
        {
            if (direction == 1)
            {
                endPos.x = Constants.maxXPos;
            }
            else
            {

                endPos.x = Constants.minXPos;
            }
            endPos.y = -startPos.y;
        }
        return endPos;
    }
}
