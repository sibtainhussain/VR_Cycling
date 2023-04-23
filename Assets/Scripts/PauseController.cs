using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool paused = false;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
    public GameObject gameHUD;

    // Start is called before the first frame update
    void Start() {
        Time.timeScale = 1f;
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (paused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        paused = false;
        Time.timeScale = 1f;
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameHUD.SetActive(true);
    }

    void Pause() {        
        paused = true;
        Time.timeScale = 0f;
        controlsMenu.SetActive(true);
        gameHUD.SetActive(false);
    }
    
    public void getSettings() {
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void getControls() {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void getQuit() {
        MainMenu.PrevScene = "GameScene";
        SceneManager.LoadScene("MainMenuScene");
    }

}

