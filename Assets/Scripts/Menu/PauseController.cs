using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public static bool paused = false;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
    public GameObject gameHUD;

    // Start is called before the first frame update
    void Start() {
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
        controlsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        gameHUD.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Pause() {        
        paused = true;
        controlsMenu.SetActive(true);
        gameHUD.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
    
    public void getSettings() {
        settingsMenu.SetActive(true);
        controlsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void getControls() {
        settingsMenu.SetActive(false);
        controlsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void getQuit() {
        MainMenu.PrevScene = "GameScene";
        SceneManager.LoadScene("MainMenuScene");
        EventSystem.current.SetSelectedGameObject(null);
    }

}

