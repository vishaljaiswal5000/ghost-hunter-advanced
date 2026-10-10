using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject car;
    public PacStudentController pacStudentController;

    // Swipe (touch, or mouse drag): the heading changes each time the finger travels this far
    private Vector2 swipeOrigin;
    private bool swiping;
    private float SwipeDistance { get { return Mathf.Max(30f, Screen.height * 0.04f); } }

    private void Update()
    {
        // load car after completion of intro
        if (car == null)
        {
            if (!LoadAssets.isIntroCompleted) return;
            car = GameObject.FindGameObjectWithTag("Car");
            if (car == null) return;
            pacStudentController = car.GetComponent<PacStudentController>();
        }

        // Control pacstudent movement until game is over.
        if (Constants.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) pacStudentController.SetInput(0);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) pacStudentController.SetInput(1);
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) pacStudentController.SetInput(2);
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) pacStudentController.SetInput(3);

        ReadSwipe();
    }

    // Touches are turned into mouse events by Unity, so one code path covers both
    private void ReadSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            swipeOrigin = Input.mousePosition;
            swiping = true;
        }
        else if (Input.GetMouseButton(0) && swiping)
        {
            Vector2 delta = (Vector2)Input.mousePosition - swipeOrigin;
            if (delta.magnitude < SwipeDistance) return;

            // 0 = Up, 1 = Down, 2 = Left, 3 = Right
            int dir = Mathf.Abs(delta.x) > Mathf.Abs(delta.y) ? (delta.x > 0 ? 3 : 2) : (delta.y > 0 ? 0 : 1);
            pacStudentController.SetInput(dir);
            swipeOrigin = Input.mousePosition;
        }
        else
        {
            swiping = false;
        }
    }
}
