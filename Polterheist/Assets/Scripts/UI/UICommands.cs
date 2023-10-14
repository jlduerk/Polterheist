using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UICommands {
    public static void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public static void QuitGame() {
        Application.Quit();
    }
}
