using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICommands : MonoBehaviour {
    public static void LoadScene(string sceneName) {
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition == null) {
            return;
        }

        sceneTransition.GoToSceneTransition(sceneName);
    }

    public static void QuitGame() {
        SceneTransition sceneTransition = FindObjectOfType<SceneTransition>();
        if (sceneTransition == null) {
            return;
        }

        sceneTransition.QuitGameTransition();
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