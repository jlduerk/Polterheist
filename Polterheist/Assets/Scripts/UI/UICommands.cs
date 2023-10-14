using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICommands : MonoBehaviour {
    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame() {
        Application.Quit();
    }

    public void Pause(bool pauseGame) {
        int timeScale = pauseGame ? 0 : 1;
        Time.timeScale = timeScale;
        //TODO?
        if (pauseGame) {
            
        }
        else {

        }
    }
}
