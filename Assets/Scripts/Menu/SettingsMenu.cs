using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public static int musicVol;
    public static int sfxVol;
    
    public void backButton() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
