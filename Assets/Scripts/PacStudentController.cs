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

    private List<CarModel> activeTweens = new List<CarModel>();
    public GameObject BonusPellet;

    public int[] currPosition = Constants.levelMapStartPos;
    public void initMovement(CarModel carModel)
    {
        this.carModel = carModel;
        mAnimator = this.carModel.Target.GetComponent<Animator>();
    }

    void Start()
    {
        mAudioSource = gameObject.GetComponent<AudioSource>();
        mAudioSource.Stop();
        mAnimator = gameObject.GetComponent<Animator>();
        mAnimator.enabled = false;

        carCollide = GameObject.FindGameObjectWithTag("Sound-Collide").GetComponent<AudioSource>();
        pelletEaten = GameObject.FindGameObjectWithTag("Sound-Pellet-Eaten").GetComponent<AudioSource>();
        powerPelletEaten = GameObject.FindGameObjectWithTag("Sound-Power-Pellet-Eaten").GetComponent<AudioSource>();

        InvokeRepeating("spawnBonusPellet", 10, 20);
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
                    mAnimator.enabled = false;
                }
            }
        }
    }

    public void startLerp(CarModel target)
    {

        float distance = Vector3.Distance(target.StartPos, target.EndPos);
        float timeFraction = (Time.time - target.StartTime) / target.Duration;

        if (timeFraction > 1)
        {
            if (lastInput != target.input)
            {
                activeTweens.Remove(target);
                mAudioSource.Stop();
                mAnimator.enabled = false;
            }
            else
            {
                Transform lastTarget = target.Target;
                currentInput = target.input;
                activeTweens.Remove(target);
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

    public bool validateInput(KeyCode input)
    {

        int row = 1;
        int col = 1;
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
            code = Constants.FulllevelMap[row, col];
        }
        catch (System.Exception)
        {
            code = -1;
        }

        bool status = checkValidation(code);
        if (status)
        {
            updateLastPosition(row, col);
        }

        return status;
    }

    public bool checkValidation(int code)
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
                return true;
            case 6:
                powerPelletEaten.Play();
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
            xPos = 3.853715f + Constants.xPos_Delta;
        }
        else
        {
            xPos = -3.721889f - Constants.xPos_Delta;
        }

        gameObject.transform.position = new Vector3(xPos, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
