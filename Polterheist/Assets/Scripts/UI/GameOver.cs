using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TeamData teamA;
    [SerializeField] private TeamData teamB;

    [SerializeField] private Image blueGhost;
    [SerializeField] private Image redGhost;
    [SerializeField] private Sprite blueGhostVictory;
    [SerializeField] private Sprite blueGhostDefeat;
    [SerializeField] private Sprite redGhostVictory;
    [SerializeField] private Sprite redGhostDefeat;

    [SerializeField] private string winText = "vict0ry!";
    [SerializeField] private string drawText = "Draw!";
    private const string MAIN_MENU_SCENE_NAME = "MainMenu";
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.GameEndEvent.AddListener(OnGameEnd);
        gameOverPanel.SetActive(false);
    }

    void OnGameEnd()
    {
        ScoreManager scoreManager = gameManager.scoreManager;

        scoreText.text = $"{scoreManager.ScoreA}-{scoreManager.ScoreB}";

        if (scoreManager.ScoreA > scoreManager.ScoreB)
        {
            victoryText.text = $"{teamA.teamName} {winText}";
            blueGhost.sprite = blueGhostVictory;
            redGhost.sprite = redGhostDefeat;
        } 
        else if (scoreManager.ScoreB > scoreManager.ScoreA)
        {
            victoryText.text = $"{teamB.teamName} {winText}";
            blueGhost.sprite = blueGhostDefeat;
            redGhost.sprite = redGhostVictory;
        } else
        {
            victoryText.text = drawText;
            blueGhost.sprite = blueGhostVictory;
            redGhost.sprite = redGhostVictory;
        }

        gameOverPanel.SetActive(true);

    }

    public void Continue() {
        UICommands.LoadScene(MAIN_MENU_SCENE_NAME);
    }
}
