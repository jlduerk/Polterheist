using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings {
    public static bool IsPaused { get; private set; }
    public static void PauseGame(bool pause) {
        IsPaused = pause;
    }
}