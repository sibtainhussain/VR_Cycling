using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenu : MonoBehaviour
{
    public void settingsButton() {
        SceneManager.LoadScene("SettingsScreen");
    }

    public void backButton() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void quitGame() {
        Application.Quit();
    }
}
