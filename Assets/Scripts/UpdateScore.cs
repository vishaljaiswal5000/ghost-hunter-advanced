using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    PacStudentController pacStudentController;
    private void Start()
    {
        pacStudentController = gameObject.GetComponent<PacStudentController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(Constants.levelMaplastPos[0]+", "+ Constants.levelMaplastPos[1]);
        if (collision.tag == "Normal-Pellet")
        {
            //update score
            updateScore(10);            
            //remove pellet
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "Power-Pellet")
        {
            // Scare ghost state
            // Start 10 time
            Constants.isGhostScared = true;            
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
        else if (collision.tag.Contains("ghost"))
        {
            if (Constants.isGhostScared)
            {
                updateScore(300);
                //play ghost dead sound
                GhostAnimator ghostAnimator = collision.gameObject.GetComponent<GhostAnimator>();
                //respawn/walking transition after 5 second
                ghostAnimator.isDead = true;
                ghostAnimator.ghostDead.Play();

            }
            else
            {
                // play car wreck sound
                pacStudentController.carWreck.Play();
                // respawn to original location
                pacStudentController.respawn();
                //remove one life
                updateLife();
            }

        }
    }

    public void updateScore(int points)
    {
        Constants.currentScore += points;

        //All pellet eaten
        if(Constants.pelletLeft == 0)
        {
            StartCoroutine("gameOver");
        }
    }


    public void updateLife()
    {
        GameObject panelLives = GameObject.FindGameObjectWithTag("Panel-Lives");
        if (panelLives != null)
        {
            if (panelLives.transform.childCount > 0)
            {
                Destroy(panelLives.transform.GetChild(panelLives.transform.childCount - 1).gameObject);
            }
            else
            {
                StartCoroutine("gameOver");
            }
        }
    }

    IEnumerator gameOver()
    {
        //Stop all movement
        pacStudentController.stopMovement();
        //Stop timer
        Constants.isGameOver = true;
        // Save Score
        saveHighScore();

        //Show game over text for 3 seconds and switch to start menu
        GameObject gameoverObj = GameObject.FindGameObjectWithTag("Text-Gameover");
        GameObject managersObj = GameObject.FindGameObjectWithTag("Managers");
        if (gameoverObj != null)
        {
            gameoverObj.GetComponent<Text>().text = "Game Over";
            yield return new WaitForSeconds(3);
            gameoverObj.GetComponent<Text>().text = "";
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
            reset();
            //if (managersObj != null)
            //{
            //SceneManager.sceneLoaded += managersObj.GetComponent<UIManager>().OnSceneLoaded;
            //}
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }
        yield return null;
    }

    public void saveHighScore()
    {
        if (PlayerPrefs.HasKey(Constants.highscoreKey))
        {
            if (PlayerPrefs.GetInt(Constants.highscoreKey) < Constants.currentScore)
            {
                commitSave();
            }
        }
        else
        {
            commitSave();
        }
    }

    public void commitSave()
    {
        PlayerPrefs.SetInt(Constants.highscoreKey, Constants.currentScore);
        PlayerPrefs.SetFloat(Constants.timeKey, Constants.playerGameTime);
        PlayerPrefs.Save();
    }

    public void reset()
    {
        Constants.isGameOver = false;
        Constants.currentScore = 0;
        Constants.playerGameTime = 0f;
        LoadAssets.isIntroCompleted = false;
    }
}
