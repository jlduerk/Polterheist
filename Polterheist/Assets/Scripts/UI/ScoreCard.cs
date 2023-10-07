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

    // Start is called before the first frame update
    void Start()
    {
        bgImage.color = teamData.teamColor;
        scoreManager = GameManager.Instance.scoreManager;
    }

    void FixedUpdate()
    {
        if (teamData.teamZone == EZone.ZoneA)
        {
            scoreText.text = scoreManager.ScoreA.ToString();
        } else if (teamData.teamZone == EZone.ZoneB)
        {
            scoreText.text = scoreManager.ScoreB.ToString();
        }
    }
}
