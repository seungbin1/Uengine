using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSet : MonoBehaviour
{
    public GameObject gameObjectTrue = null;
    public GameObject gameObjectFalse = null;
    public void StopUI()
    {
        Time.timeScale = 0;
        gameObjectFalse.gameObject.SetActive(false);
        gameObjectTrue.gameObject.SetActive(true);
    }
    public void SettingUI()
    {
        Time.timeScale = 1;
        gameObjectFalse.gameObject.SetActive(false);
        gameObjectTrue.gameObject.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1;
        gameObjectFalse.gameObject.SetActive(false);
        gameObjectTrue.gameObject.SetActive(true);
    }
    public void SetUI()
    {
        gameObject.gameObject.SetActive(true);
    }
}
