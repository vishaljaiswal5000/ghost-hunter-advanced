using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject car;
    public PacStudentController pacStudentController;

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
    }
}
