using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonManagerPlayer : MonoBehaviour
{
    public GameObject PlanePause;
    public GameObject PlaneSet;
    public GameObject PlaneQuit;
    public void Continue()
    {
        PlanePause.SetActive(false);
        Time.timeScale = 1;
    }
    public void OpenSet0()
    {
        PlaneSet.SetActive(true);
        PlanePause.SetActive(false);
    }
    public void CloseSet0()
    {
        PlaneSet.SetActive(false);
        PlanePause.SetActive(true);
    }
    public void OpenQuit0()
    {
        PlaneQuit.SetActive(true);
        PlanePause.SetActive(false);
    }
    public void CloseQuit0()
    {
        PlaneQuit.SetActive(false);
        PlanePause.SetActive(true);
    }
    public void ExitGame0()
    {
        Application.Quit();
    }
    public void Home()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("Menu");
        if (GameObject.Find("StaticObject"))
            Destroy(GameObject.Find("StaticObject"));

    }
}
