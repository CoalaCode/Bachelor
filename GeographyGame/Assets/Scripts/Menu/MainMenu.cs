/*******************************************************************
* Author            : Max Schneider
* Copyright         : MIT License
* File Name         : MainMenu.cs
* Description       : This file contains the logic for the menus.
*
/******************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartExplorerMode()
    {
        SceneManager.LoadScene("ExplorerMode");
    }

    public void StartChallengeMode()
    {
        SceneManager.LoadScene("ChallengeMode");
    }

    public void StartSimulationMode()
    {
        SceneManager.LoadScene("SimulationMode");
    }

    public void StartMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

   
}

