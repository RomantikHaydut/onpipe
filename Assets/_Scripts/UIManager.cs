using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject gameOverPanel;

    public TMP_Text scoreText;

    public TMP_Text bestScoreText; // in game over scene

    public TMP_Text timeText;

    public Image healthBar;

    public void OpenGameOverPanel()
    {
        bestScoreText.text = DataManager.bestScore.ToString();
        gameOverPanel.SetActive(true);
        
    }

    public void AddScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void CalculateTime(int time)
    {
        timeText.text = "Time : " + time.ToString();
    }

    public void ControlHealthBar(float amount)
    {
        healthBar.fillAmount = amount / 100;
    }

}
