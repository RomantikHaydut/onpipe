using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_Text bestScoreText;

    private void Start()
    {
        DataManager.Instance.LoadBestScore();
        DisplayBestScore();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void GoRingsScene()
    {
        SceneManager.LoadScene(2);
    }

    void DisplayBestScore()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "H i g h  s c o r e  : " + DataManager.bestScore;
        }

    }
}
