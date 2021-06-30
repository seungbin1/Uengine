using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public string sceneName=null;
    public void SceneMove()
    {
        if (sceneName != null)
        {
            PlayerMovement.Init();
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }
    }
    public void Exit()
    {
        Application.Quit();
    }
}
