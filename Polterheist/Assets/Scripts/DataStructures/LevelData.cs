using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (menuName = "Polterheist/LevelData", fileName = "New LevelData")]
public class LevelData : ScriptableObject {
    public string name;
    public Scene scene;
    public Sprite previewImage;
}