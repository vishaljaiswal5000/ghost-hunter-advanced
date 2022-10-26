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
    private float gameTimer = 0.0f;
    //private float ghostScaredTimer = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateGameTime();
        showGhostScaredTimer();
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
        if (scene.buildIndex == 1)
        {
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


        }
    }

    void updateGameTime()
    {
        if (gameTimerField != null && LoadAssets.isIntroCompleted)
        {
            gameTimer += Time.deltaTime;
            string completeTimeStr = "Time: " + TimeSpan.FromSeconds(gameTimer).ToString("mm':'ss':'ff");
            gameTimerField.text = completeTimeStr;
        }
    }

    void showGhostScaredTimer()
    {
        if (ghostScaredTimerField != null)
        {
            if (false)
            {
                ghostScaredTimerField.enabled = true;
            }
            else
            {
                ghostScaredTimerField.enabled = false;
            }

        }

    }
}
