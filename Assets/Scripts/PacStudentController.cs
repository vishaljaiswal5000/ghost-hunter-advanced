using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    public CarModel carModel;
    public KeyCode lastInput;
    public KeyCode currentInput;
    private Animator mAnimator;
    private AudioSource mAudioSource;

    public AudioSource carCollide;
    public AudioSource pelletEaten;
    public AudioSource powerPelletEaten;
    public AudioSource carWreck;

    private List<CarModel> activeTweens = new List<CarModel>();
    public GameObject BonusPellet;

    public ParticleSystem particleEffect;
    public int[] currPosition;
    public Vector3 carBeginningPosition;
    public Vector3 carBeginningRotation;
    public void init()
    {
        // Update starting position
        currPosition = Constants.levelMapStartPos;
        carBeginningPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);

        // Load animator and car audio
        mAudioSource = gameObject.GetComponent<AudioSource>();
        mAudioSource.Stop();
        mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.enabled = false;

        // Load audio sources
        carCollide = GameObject.FindGameObjectWithTag("Sound-Collide").GetComponent<AudioSource>();
        pelletEaten = GameObject.FindGameObjectWithTag("Sound-Pellet-Eaten").GetComponent<AudioSource>();
        powerPelletEaten = GameObject.FindGameObjectWithTag("Sound-Power-Pellet-Eaten").GetComponent<AudioSource>();
        carWreck = GameObject.FindGameObjectWithTag("Sound-Wreck").GetComponent<AudioSource>();

        // Load car(pacstudent) particle system
        particleEffect = GameObject.FindGameObjectWithTag("Particle-Effect-Drive").GetComponent<ParticleSystem>();
        particleEffect.Stop();

        // Spawn Bonus pellet until game over
        if (!Constants.isGameOver)
        {
            InvokeRepeating("spawnBonusPellet", 10, 20);
        }
    }

    void Start()
    {
        init();
    }


    void Update()
    {
        carMovement();
    }

    public void carMovement()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            if (activeTweens[i] != null)
            {
                if (!mAnimator.GetBool("isDead"))
                {
                    mAnimator.enabled = true;
                    if (!mAudioSource.isPlaying)
                    {
                        mAudioSource.Play();
                    }
                    startLerp(activeTweens[i]);
                }
                else
                {
                    mAudioSource.Stop();
                    mAnimator.enabled = true;
                }
            }
        }
    }

    public void startLerp(CarModel target)
    {
        // calculate distance between start and end position 
        float distance = Vector3.Distance(target.StartPos, target.EndPos);
        float timeFraction = (Time.time - target.StartTime) / target.Duration;

        if (timeFraction > 1)
        {
            if (lastInput != target.input)
            {
                if (validateInput(lastInput, true))
                {
                    AddTween(target.Target, lastInput);
                    activeTweens.Remove(target);
                }
                else
                {
                    // Stop movement and sound
                    activeTweens.Remove(target);
                    mAudioSource.Stop();
                    mAnimator.enabled = false;
                }
            }
            else
            {
                Transform lastTarget = target.Target;
                currentInput = target.input;

                // remove last movement tween
                activeTweens.Remove(target);

                // continue in same direction
                AddTween(lastTarget, currentInput);
            }
        }
        else
        {
            target.Target.position = Vector3.Lerp(target.StartPos, target.EndPos, timeFraction);
        }
    }

    public float EaseIn(float t)
    {
        return Mathf.Pow(t, 3);
    }
    public bool AddTween(Transform targetObject, KeyCode input)
    {
        if (!TweenExists(targetObject))
        {
            if (validateInput(input))
            {
                activeTweens.Add(resolveInput(targetObject, input));
                lastInput = input;
                return true;
            }
            else
            {
                mAudioSource.Stop();
                mAnimator.enabled = false;
            }
        }

        return false;
    }

    public CarModel resolveInput(Transform targetObject, KeyCode input)
    {
        CarModel car;
        switch (input)
        {
            case KeyCode.W:
                car = moveUp(targetObject);
                break;

            case KeyCode.S:
                car = moveDown(targetObject);
                break;

            case KeyCode.A:
                car = moveLeft(targetObject);
                break;

            case KeyCode.D:
                car = moveRight(targetObject);
                break;

            default:
                car = moveRight(targetObject);
                break;
        }

        return car;
    }

    public CarModel moveLeft(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x - Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 90.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.A);
    }

    public CarModel moveRight(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x + Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 270.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.D);
    }

    public CarModel moveUp(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y - Constants.yPos_Delta, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.W);
    }

    public CarModel moveDown(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y + Constants.yPos_Delta, targetObject.position.z);
        targetObject.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 180.0f));
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.S);
    }

    public bool TweenExists(Transform target)
    {
        // Linq instead of loop
        //CarModel tween = activeTweens.Find(x => x.input == lastInput);
        CarModel tween = activeTweens.Find(x => x.Target == target);

        if (tween != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool validateInput(KeyCode input, bool isTest = false)
    {
        // starting position
        int row = 1;
        int col = 1;

        // Update row and column according to user input
        if (input == KeyCode.D)
        {
            row = currPosition[0];
            col = currPosition[1] + 1;

        }
        else if (input == KeyCode.A)
        {
            row = currPosition[0];
            col = currPosition[1] - 1;
        }
        else if (input == KeyCode.W)
        {
            row = currPosition[0] - 1;
            col = currPosition[1];
        }
        else if (input == KeyCode.S)
        {
            row = currPosition[0] + 1;
            col = currPosition[1];
        }

        // check for tunnel entrance
        // true = entered from left
        // false = entered from right

        if (row == 14 || row == 15)
        {
            if (col == 0)
            {
                col = 27;
                teleport(true);
            }
            else if (col == 27)
            {
                col = 0;
                teleport(false);
            }
        }

        int code;
        try
        {
            // get element from level map (pellet, power pellet or walls)
            code = Constants.FulllevelMap[row, col];
        }
        catch (System.Exception)
        {
            code = -1;
        }

        // Check if car(pacStudent) can move or not 
        bool status = checkValidation(code, row, col);
        if (status && !isTest)
        {
            particleEffect.Play();
            updateLastPosition(row, col);
        }
        else
        {
            particleEffect.Stop();
        }

        // Count total pellet left
        //checkTotalPellet();

        return status;
    }

    public bool checkValidation(int code, int row, int col)
    {
        switch (code)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 7:
                carCollide.Play();
                return false;
            case 5:
                pelletEaten.Play();
                Constants.FulllevelMap[row, col] = 0;
                return true;
            case 6:
                powerPelletEaten.Play();
                Constants.FulllevelMap[row, col] = 0;
                return true;
            case 0:
                return true;
            case -1:
                return true;
            default:
                return false;
        }
    }

    public void updateLastPosition(int row, int col)
    {
        Constants.levelMaplastPos[0] = currPosition[0] = row;
        Constants.levelMaplastPos[1] = currPosition[1] = col;
    }

    public void spawnBonusPellet()
    {
        Instantiate(BonusPellet, Vector3.zero, BonusPellet.transform.rotation);
    }

    public void teleport(bool isEnteredFromLeft)
    {
        float xPos;
        if (isEnteredFromLeft)
        {
            xPos = 4.1f + Constants.xPos_Delta;
        }
        else
        {
            xPos = -4.1f - Constants.xPos_Delta;
        }

        gameObject.transform.position = new Vector3(xPos, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    public void respawn()
    {
        StartCoroutine("respawning");
    }

    IEnumerator respawning()
    {
        particleEffect.Stop();
        mAnimator.SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        mAnimator.SetBool("isDead", false);
        activeTweens = new List<CarModel>();
        transform.position = carBeginningPosition;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 270));
        updateLastPosition(1, 1);
        currPosition[0] = 1;
        currPosition[1] = 1;
        yield return null;
    }

    public void stopMovement()
    {
        activeTweens.Clear();
    }

    public void checkTotalPellet()
    {
        int totalPellet = 0;
        GameObject[] pellets = GameObject.FindGameObjectsWithTag("Normal-Pellet");
        totalPellet = pellets.Length - 1;
        Constants.pelletLeft = totalPellet;
    }
}
