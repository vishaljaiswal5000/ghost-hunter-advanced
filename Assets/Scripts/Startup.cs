using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    public void onLevelOneClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void onLevelTwoClick()
    {

    }
}
