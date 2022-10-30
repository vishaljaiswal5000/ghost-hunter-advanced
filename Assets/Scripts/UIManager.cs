using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    Text gameTimerField;
    Text ghostScaredTimerField;
    Text countdownField;
    Text scoreField;
    Text highscoreField;
    Text highscoreTimeField;
    Text pelletLeftField;

    private float gameTimer = 0.0f;
    public bool isGhostTimerStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        //reset();
        GameObject highscore = GameObject.FindGameObjectWithTag("Text-Highscore");
        if (highscore != null)
        {
            highscoreField = highscore.GetComponent<Text>();
        }

        GameObject highscoreTime = GameObject.FindGameObjectWithTag("Text-Highscore-Time");
        if (highscoreTime != null)
        {
            highscoreTimeField = highscoreTime.GetComponent<Text>();
        }

        setHighscore();
    }

    // Update is called once per frame
    void Update()
    {
        updateGameTime();
        showGhostScaredTimer();
        updateScore();
    }

    public void onLevelOneClick()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine("loadScene", 1);

    }

    public void onLevelTwoClick()
    {

    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public IEnumerator loadScene(int sceneIndex)
    {
        DontDestroyOnLoad(gameObject);
        //yield return new WaitForSeconds(2);
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        SceneManager.sceneLoaded += OnSceneLoaded;
        //asyncOperation.completed += (param) => { HideLoadingScreen(); };
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            //reset();
            GameObject highscore = GameObject.FindGameObjectWithTag("Text-Highscore");
            if (highscore != null)
            {
                highscoreField = highscore.GetComponent<Text>();
            }

            GameObject highscoreTime = GameObject.FindGameObjectWithTag("Text-Highscore-Time");
            if (highscoreTime != null)
            {
                highscoreTimeField = highscoreTime.GetComponent<Text>();
            }

            setHighscore();

        }
        else if (scene.buildIndex == 1)
        {
            // initiating game settings
            Constants.init(originalFulllevelMap);

            // Saving ui reference
            GameObject quitButton = GameObject.FindGameObjectWithTag("QuitButton");
            if (quitButton != null)
            {
                Button quitbtn = quitButton.GetComponent<Button>();
                quitbtn.onClick.AddListener(QuitGame);
            }

            GameObject timer = GameObject.FindGameObjectWithTag("Text-Timer");
            if (timer != null)
            {
                gameTimerField = timer.GetComponent<Text>();
            }

            GameObject ghostScaredtimer = GameObject.FindGameObjectWithTag("Text-Ghost-Scared-Timer");
            if (ghostScaredtimer != null)
            {
                ghostScaredTimerField = ghostScaredtimer.GetComponent<Text>();
            }

            GameObject score = GameObject.FindGameObjectWithTag("Text-Score");
            if (score != null)
            {
                scoreField = score.GetComponent<Text>();
            }

            GameObject countDown = GameObject.FindGameObjectWithTag("Text-CountDown");
            if (countDown != null)
            {
                countdownField = countDown.GetComponent<Text>();
            }

            GameObject pelletLeft = GameObject.FindGameObjectWithTag("Text-Pellet-Left");
            if (pelletLeft != null)
            {
                pelletLeftField = pelletLeft.GetComponent<Text>();
            }


            // Start countdown
            StartCoroutine("startCountdown");
        }
    }

    IEnumerator ghostScareCountdown()
    {
        if (ghostScaredTimerField != null && Constants.isGhostScared)
        {
            isGhostTimerStarted = true;
            ghostScaredTimerField.text = "Scared Time: 10";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 9";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 8";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 7";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 6";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 5";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 4";
            yield return new WaitForSeconds(1.0f);
            //Recovery state
            Constants.isGhostRecovering = true;
            ghostScaredTimerField.text = "Scared Time: 3";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 2";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "Scared Time: 1";
            yield return new WaitForSeconds(1.0f);
            ghostScaredTimerField.text = "";
            yield return null;
            isGhostTimerStarted = false;
            Constants.isGhostScared = false;
            Constants.isGhostRecovering = false;
        }
    }

    IEnumerator startCountdown()
    {
        if (countdownField != null)
        {
            countdownField.text = "3";
            yield return new WaitForSeconds(1.0f);
            countdownField.text = "2";
            yield return new WaitForSeconds(1.0f);
            countdownField.text = "1";
            yield return new WaitForSeconds(1.0f);
            countdownField.text = "Go!";
            // start the game here
            yield return new WaitForSeconds(1.0f);
            countdownField.text = "";
            yield return null;
        }
    }

    // Update game time
    void updateGameTime()
    {
        if (gameTimerField != null && LoadAssets.isIntroCompleted && !Constants.isGameOver)
        {
            gameTimer += Time.deltaTime;
            Constants.playerGameTime = gameTimer;
            string completeTimeStr = "Time: " + TimeSpan.FromSeconds(gameTimer).ToString("mm':'ss':'ff");
            gameTimerField.text = completeTimeStr;
        }
    }

    // Start ghost scare countdown
    public void showGhostScaredTimer()
    {
        if (ghostScaredTimerField != null && !isGhostTimerStarted)
        {
            StartCoroutine("ghostScareCountdown");
        }

    }

    //Update score and pellet left field
    public void updateScore()
    {
        if (scoreField != null)
        {
            scoreField.text = string.Format("Score: {0}", Constants.currentScore.ToString());
        }

        if (pelletLeftField != null)
        {
            pelletLeftField.text = string.Format("Pellet Left: {0}", Constants.pelletLeft.ToString());
        }
    }

    // Reset ui level variables
    public void reset()
    {
        Constants.isGameOver = false;
        Constants.currentScore = 0;
        Constants.playerGameTime = 0f;
        LoadAssets.isIntroCompleted = false;
    }


    // Update high score on the screen
    public void setHighscore()
    {
        if (PlayerPrefs.HasKey(Constants.highscoreKey))
        {
            highscoreField.text = "HighScore: " + PlayerPrefs.GetInt(Constants.highscoreKey).ToString();
        }

        if (PlayerPrefs.HasKey(Constants.timeKey))
        {
            highscoreTimeField.text = "Time: " + TimeSpan.FromSeconds(PlayerPrefs.GetFloat(Constants.timeKey)).ToString("mm':'ss':'ff");
        }
    }

    // Full level map
    public int[,] originalFulllevelMap ={
                     {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2},
                     {1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1},
                     {0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2},
                     {0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0},
                     {2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0, 0, 0, 0, 4, 0, 3, 3, 5, 1, 2, 2, 2, 2, 2},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0, 0, 4, 4, 3, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4, 5, 2, 0, 0, 0, 0, 0},
                     {0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3, 3, 0, 3, 4, 4, 3, 4, 5, 2, 0, 0, 0, 0, 0},
                     {1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4, 4, 0, 3, 4, 4, 3, 4, 5, 1, 2, 2, 2, 2, 1},
                     {2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4, 4, 5, 4, 0, 0, 0, 4, 5, 4, 0, 0, 4, 6, 2},
                     {2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 4, 3, 5, 3, 4, 4, 3, 5, 2},
                     {2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 2},
                     {1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7, 7, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1},
                     };
}
