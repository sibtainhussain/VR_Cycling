using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void startGame() {
        SceneManager.LoadScene("GameScene");
    }

    public void getControls() {
        
        SceneManager.LoadScene("ControlsScene");
    }

    public void getSettings() {
        SceneManager.LoadScene("SettingsScreen");
    }

    public void getStats() {
        SceneManager.LoadScene("StatsScreen");
    }

    public void quitGame() {
        Application.Quit();
    }
}
