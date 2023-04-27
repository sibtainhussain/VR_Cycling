using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelectMenu : MonoBehaviour
{
    public void map1Button() {
        SceneManager.LoadScene("GameScene");
    }

    public void map2Button() {
        SceneManager.LoadScene("WaterfallTrack");
    }

    public void backButton() {
        SceneManager.LoadScene("MainMenuScene");
    }
}
