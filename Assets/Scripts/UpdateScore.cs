using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(Constants.levelMaplastPos[0]+", "+ Constants.levelMaplastPos[1]);
        if (collision.tag == "Normal-Pellet")
        {
            //update score
            updateScore(10);
            // remove pellet from map
            Constants.FulllevelMap[Constants.levelMaplastPos[0], Constants.levelMaplastPos[1]] = 0;
            //remove pellet
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Power-Pellet")
        {
            // Scare ghost state
            // Start 10 time
            // remove pellet from map
            Constants.FulllevelMap[Constants.levelMaplastPos[0], Constants.levelMaplastPos[1]] = 0;
            //remove pellet
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Bonus-Pellet")
        {
            //update score
            updateScore(100);
            // remove pellet from map
            Constants.FulllevelMap[Constants.levelMaplastPos[0], Constants.levelMaplastPos[1]] = 0;
            //remove pellet
            Destroy(collision.gameObject);
        }
    }

    public void updateScore(int points)
    {
        Constants.currentScore += points;
    }

}
