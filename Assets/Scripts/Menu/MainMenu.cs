using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static string PrevScene;
    
    public void startGame() {
        SceneManager.LoadScene("MapSelectScene");
        PrevScene = "MainMenuScene";
    }

    public void getControls() {
        SceneManager.LoadScene("ControlsScene");
        PrevScene = "MainMenuScene";
    }

    public void getSettings() {
        SceneManager.LoadScene("SettingsScreen");
        PrevScene = "MainMenuScene";
    }

    public void getStats() {
        SceneManager.LoadScene("StatsScreen");
        PrevScene = "MainMenuScene";
    }

    public void quitGame() {
        EventSystem.current.SetSelectedGameObject(null);
        Application.Quit();
    }
}
