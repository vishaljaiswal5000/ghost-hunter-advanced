using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    Vector3 startPos;
    int[] currLocation = new int[2];
    Vector3 lastPos;
    private Animator mAnimator;
    private List<CarModel> activeTweens = new List<CarModel>();


    public KeyCode lastInput;
    public KeyCode currentInput;


    // Start is called before the first frame update
    void Start()
    {
        mAnimator = gameObject.GetComponent<Animator>();
        init();
    }

    // Update is called once per frame
    void Update()
    {
        ghostMovement();
    }

    // manage all tweens and start movement
    public void ghostMovement()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            if (activeTweens[i] != null)
            {
                if (!mAnimator.GetBool("isDead"))
                {
                    mAnimator.enabled = true;
                    startLerp(activeTweens[i]);
                }
                else
                {
                    mAnimator.enabled = true;
                }
            }
        }
    }

    // Perform Interpolation
    public void startLerp(CarModel target)
    {
        // calculate distance between start and end position 
        float distance = Vector3.Distance(target.StartPos, target.EndPos);
        float timeFraction = (Time.time - target.StartTime) / target.Duration;

        if (timeFraction > 1)
        {
            target.Target.position = target.EndPos;
            Transform tempTarget = target.Target;
            activeTweens.Remove(target);
            createTween();
        }
        else
        {
            target.Target.position = Vector3.Lerp(target.StartPos, target.EndPos, timeFraction);
        }
    }

    public void init()
    {
        if (gameObject.name.Contains("Blue"))
        {
            currLocation[0] = 13;
            currLocation[1] = 16;
        }
        else if (gameObject.name.Contains("Green"))
        {
            currLocation[0] = 16;
            currLocation[1] = 11;
        }
        else if (gameObject.name.Contains("Yellow"))
        {
            currLocation[0] = 13;
            currLocation[1] = 11;
        }
        else if (gameObject.name.Contains("Red"))
        {
            currLocation[0] = 16;
            currLocation[1] = 16;
        }

        startPos = gameObject.transform.position;
        createTween();

    }

    public void createTween()
    {
        KeyCode input = randomInput();
        if (validateInput(input, true)){

            bool isCreated = AddTween(gameObject.transform, input);
            if (!isCreated)
            {
                createTween();
            }
        }else
        {
            createTween();
        }
    }

    public CarModel moveLeft(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x - Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.A);
    }

    public CarModel moveRight(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x + Constants.xPos_Delta, targetObject.position.y, targetObject.position.z);
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.D);
    }

    public CarModel moveUp(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y - Constants.yPos_Delta, targetObject.position.z);
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.W);
    }

    public CarModel moveDown(Transform targetObject)
    {
        Vector3 endPos = new Vector3(targetObject.position.x, targetObject.position.y + Constants.yPos_Delta, targetObject.position.z);
        return new CarModel(targetObject, targetObject.position, endPos, Time.time, Constants.carMovementDuration, KeyCode.S);
    }


    // Validate user input is executable or not
    public bool validateInput(KeyCode input, bool isTest = false)
    {
        // starting position
        int row = 1;
        int col = 1;

        // Update row and column according to user input
        if (input == KeyCode.D)
        {
            row = currLocation[0];
            col = currLocation[1] + 1;

        }
        else if (input == KeyCode.A)
        {
            row = currLocation[0];
            col = currLocation[1] - 1;
        }
        else if (input == KeyCode.W)
        {
            row = currLocation[0] - 1;
            col = currLocation[1];
        }
        else if (input == KeyCode.S)
        {
            row = currLocation[0] + 1;
            col = currLocation[1];
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
            updateLastPosition(row, col);
        }

        return status;
    }

    // Check whether next position is feasible to move
    public bool checkValidation(int code, int row, int col)
    {
        switch (code)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 7:
                return false;
            case 5:
                return true;
            case 6:
                return true;
            case 0:
                return true;
            case -1:
                return true;
            default:
                return false;
        }
    }

    // Teleport pacstudent through tunnel
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

    public KeyCode randomInput()
    {
        int selection = Random.Range(0, 4);
        KeyCode keyCode;
        switch (selection)
        {
            case 0:
                keyCode = KeyCode.W;
                break;
            case 1:
                keyCode = KeyCode.S;
                break;
            case 2:
                keyCode = KeyCode.A;
                break;
            case 3:
                keyCode = KeyCode.D;
                break;
            default:
                keyCode = KeyCode.A;
                break;
        }

        lastInput = keyCode;
        return keyCode;
    }


    // Verify and Add tween in the list
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
                mAnimator.enabled = false;
            }
        }

        return false;
    }

    // Resolve user inputs
    public CarModel resolveInput(Transform targetObject, KeyCode input)
    {
        CarModel ghost;
        switch (input)
        {
            case KeyCode.W:
                ghost = moveUp(targetObject);
                break;

            case KeyCode.S:
                ghost = moveDown(targetObject);
                break;

            case KeyCode.A:
                ghost = moveLeft(targetObject);
                break;

            case KeyCode.D:
                ghost = moveRight(targetObject);
                break;

            default:
                ghost = moveRight(targetObject);
                break;
        }

        return ghost;
    }


    // Check if tween exists in the list
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


    public void updateLastPosition(int row, int col)
    {
        currLocation[0] = row;
        currLocation[1] = col;
    }


    // Start coroutine of Respawn pacstudent 
    public void respawn()
    {
        StartCoroutine("respawning");
    }

    // Respawn pacstudent after dead state
    IEnumerator respawning()
    {        
        mAnimator.SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        mAnimator.SetBool("isDead", false);
        activeTweens = new List<CarModel>();
        transform.position = startPos;
        init();
        yield return null;
    }
}
