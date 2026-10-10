using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Lives on the car: reacts to pellets and ghosts, tracks score, lives and game over
public class UpdateScore : MonoBehaviour
{
    PacStudentController pacStudentController;

    private void Start()
    {
        pacStudentController = GetComponent<PacStudentController>();
    }

    // Manage collisions Triggers
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Constants.isGameOver) return;

        if (collision.CompareTag("Normal-Pellet"))
        {
            updateScore(10);
            Play(pacStudentController.pelletEaten);
            Destroy(collision.gameObject);

            Constants.pelletLeft--;
            if (Constants.pelletLeft <= 0)
            {
                StartCoroutine(gameOver("You Win!"));
            }
        }
        else if (collision.CompareTag("Power-Pellet"))
        {
            // Scare ghosts, (re)start the 10 second timer
            Constants.StartScare();
            Play(pacStudentController.powerPelletEaten);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Bonus-Pellet"))
        {
            updateScore(100);
            Destroy(collision.gameObject);
        }
        else
        {
            HitGhost(collision);
        }
    }

    // Ghost sitting on us when it stops being scared (or we stop being safe) still counts
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Constants.isGameOver) return;
        HitGhost(collision);
    }

    private void HitGhost(Collider2D collision)
    {
        GhostController ghost = collision.GetComponent<GhostController>();
        if (ghost == null || ghost.state != GhostState.Normal) return;

        if (ghost.IsScared)
        {
            updateScore(300);
            ghost.Eaten();
            GhostAnimator ghostAnimator = collision.GetComponent<GhostAnimator>();
            if (ghostAnimator != null) Play(ghostAnimator.ghostDead);
        }
        else if (!pacStudentController.Invulnerable)
        {
            Play(pacStudentController.carWreck);
            pacStudentController.respawn();
            updateLife();
        }
    }

    private static void Play(AudioSource source)
    {
        if (source != null) source.Play();
    }

    // Update player score in constants (shared) class
    public void updateScore(int points)
    {
        Constants.currentScore += points;
    }

    // Remove one life; game over when none are left
    public void updateLife()
    {
        Constants.lives--;

        GameObject panelLives = GameObject.FindGameObjectWithTag("Panel-Lives");
        if (panelLives != null && Constants.lives >= 0 && Constants.lives < panelLives.transform.childCount)
        {
            panelLives.transform.GetChild(Constants.lives).gameObject.SetActive(false);
        }

        if (Constants.lives <= 0)
        {
            StartCoroutine(gameOver("Game Over"));
        }
    }

    // Perform game over procedure
    IEnumerator gameOver(string message)
    {
        if (Constants.isGameOver) yield break;

        //Stop all movement and the timer
        Constants.isGameOver = true;
        pacStudentController.stopMovement();
        saveHighScore();

        //Show message for 3 seconds and switch to start menu
        GameObject gameoverObj = GameObject.FindGameObjectWithTag("Text-Gameover");
        if (gameoverObj != null)
        {
            gameoverObj.GetComponent<Text>().text = message;
        }
        yield return new WaitForSeconds(3);

        Constants.init();
        SceneManager.LoadScene(0);
    }

    // verify and Save high score in PlayerPrefs
    public void saveHighScore()
    {
        if (!PlayerPrefs.HasKey(Constants.highscoreKey) || PlayerPrefs.GetInt(Constants.highscoreKey) < Constants.currentScore)
        {
            PlayerPrefs.SetInt(Constants.highscoreKey, Constants.currentScore);
            PlayerPrefs.SetFloat(Constants.timeKey, Constants.playerGameTime);
            PlayerPrefs.Save();
        }
    }
}
