using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreCard : MonoBehaviour
{
    public TeamData teamData;
    [SerializeField] private Image bgImage;
    private ScoreManager scoreManager;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI teamText;
    private GameManager gameManager;

    [SerializeField] private float MoveTweenY = 161f;
    [SerializeField] private float MoveTweenDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        bgImage.color = teamData.teamColor;
        teamText.text = teamData.teamName;
        scoreManager = GameManager.Instance.scoreManager;
        gameManager.GameStartEvent.AddListener(GameStartTween);
        gameManager.GameEndEvent.AddListener(GameEndTween);


    }

    void GameStartTween()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(MoveTweenY, MoveTweenDuration));
    }

    void GameEndTween()
    {
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOMoveY(-MoveTweenY, MoveTweenDuration));
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
