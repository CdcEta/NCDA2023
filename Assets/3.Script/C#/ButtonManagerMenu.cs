using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ButtonManagerMenu : MonoBehaviour
{
    public static bool startGame;
    public GameObject PlaneQuit;

    public void Start()
    {
        startGame = true; 
    }

    //public GameObject pauseFirstButton, optionsFirstButton, optionsCloseButton;
    public void StartGame()
    {
        SceneManager.LoadScene("NewVillage");

        Time.timeScale = 1;
    }
    public void ToSet()
    {
        SceneManager.LoadScene("Set");
        Time.timeScale = 1;
    }
    public void toMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void OpenQuit()
    {
        PlaneQuit.SetActive(true);
    }
    public void CloseQuit()
    {
        PlaneQuit.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
