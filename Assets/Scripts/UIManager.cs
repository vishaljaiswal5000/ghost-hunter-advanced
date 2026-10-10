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

    // The UIManager survives the switch to the game scene; a fresh one comes with every start scene
    static UIManager instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            // new start scene: the old (persistent) one is replaced, our buttons point at us
            instance.enabled = false;
            Destroy(instance.gameObject);
        }
        instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
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
        startLevel(1);
    }

    public void onLevelTwoClick()
    {
        startLevel(2);
    }

    void startLevel(int level)
    {
        Constants.level = level;
        DontDestroyOnLoad(gameObject);
        StartCoroutine("loadScene", 1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public IEnumerator loadScene(int sceneIndex)
    {
        DontDestroyOnLoad(gameObject);
        //yield return new WaitForSeconds(2);
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //asyncOperation.completed += (param) => { HideLoadingScreen(); };
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
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
            Constants.init();
            gameTimer = 0f;

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

    // Run the ghost scared timer (10 seconds, last 3 are the "recovering" phase)
    public void showGhostScaredTimer()
    {
        if (Constants.isGhostScared && LoadAssets.isIntroCompleted && !Constants.isGameOver)
        {
            Constants.scaredTimeLeft -= Time.deltaTime;
            Constants.isGhostRecovering = Constants.scaredTimeLeft <= Constants.scaredRecoveringTime;

            if (Constants.scaredTimeLeft <= 0f)
            {
                Constants.isGhostScared = false;
                Constants.isGhostRecovering = false;
            }
        }

        if (ghostScaredTimerField != null)
        {
            ghostScaredTimerField.text = Constants.isGhostScared
                ? "Scared Time: " + Mathf.CeilToInt(Constants.scaredTimeLeft)
                : "";
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

    // Update high score on the screen
    public void setHighscore()
    {
        if (highscoreField != null && PlayerPrefs.HasKey(Constants.highscoreKey))
        {
            highscoreField.text = "HighScore: " + PlayerPrefs.GetInt(Constants.highscoreKey).ToString();
        }

        if (highscoreTimeField != null && PlayerPrefs.HasKey(Constants.timeKey))
        {
            highscoreTimeField.text = "Time: " + TimeSpan.FromSeconds(PlayerPrefs.GetFloat(Constants.timeKey)).ToString("mm':'ss':'ff");
        }
    }
}
