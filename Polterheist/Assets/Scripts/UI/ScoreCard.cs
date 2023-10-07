using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCard : MonoBehaviour
{
    public TeamData teamData;
    [SerializeField] private Image bgImage;
    private ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        bgImage.color = teamData.teamColor;
        scoreManager = GameManager.Instance.scoreManager;
    }

    void FixedUpdate()
    {
        if (gameManager.GameInProgress) // Only do scoring when game has started
        {
            if (teamData.teamZone == EZone.ZoneA)
            {
                scoreText.text = scoreManager.ScoreA.ToString();
            }
            else if (teamData.teamZone == EZone.ZoneB)
            {
                scoreText.text = scoreManager.ScoreB.ToString();
            }
        }

    }
}
