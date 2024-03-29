using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenuUI;

    private void Awake() {
        pauseMenuUI = GameObject.Find("PauseScreen");
    }

    // Start is called before the first frame update
    void Start() {
        pauseMenuUI.SetActive(false);
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
        pauseMenuUI.SetActive(false);
        paused = false;        
    }

    void Pause() {        
        pauseMenuUI.SetActive(true);
        paused = true;
    }
    
    public void getSettings() {
        MainMenu.PrevScene = "GameScene";
        SceneManager.LoadScene("SettingsScreen");
    }

    public void getQuit() {
        MainMenu.PrevScene = "GameScene";
        SceneManager.LoadScene("MainMenuScene");
    }

}
