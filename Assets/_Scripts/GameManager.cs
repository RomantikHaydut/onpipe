using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isPlayerAlive = true;

    public float time;

    UIManager uiManager;

    public static int level; // Difficulty.

    private float timeForDifficulty;

    CameraController cameraController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        isPlayerAlive = true;

        uiManager = FindObjectOfType<UIManager>();
        time = 0;
    }

    private void Start()
    {
        level = 1; // Means difficulty is easy.
        timeForDifficulty = 0;
        cameraController = FindObjectOfType<CameraController>();
    }

    private void Update()
    {
        if (isPlayerAlive)
        {
            CalculateTime();
            DifficultyTimer();
        }

    }

    void CalculateTime()
    {
        time += Time.deltaTime;
        uiManager.CalculateTime((int)time);
        
    }

    private void DifficultyTimer()
    {
        timeForDifficulty += Time.deltaTime;
        if (level < 6)
        {
            if ((int)timeForDifficulty % 15 == 0 && timeForDifficulty >= 15)
            {
                level++;
                timeForDifficulty = 0;
                Difficulty();
            }
        }
        else
        {
            if ((int)timeForDifficulty % 5 == 0 && timeForDifficulty >= 5)
            {
                level++;
                timeForDifficulty = 0;
                Difficulty();
            }
        }
    }

    private void Difficulty()
    {
        if (level == 1)
        {
            cameraController.difficultyFactor = 1;
        }
        else if (level == 2)
        {
            cameraController.difficultyFactor = 1.2f;
        }
        else if (level == 3)
        {
            cameraController.difficultyFactor = 1.4f;
        }
        else if (level == 4)
        {
            cameraController.difficultyFactor = 1.6f;
        }
        else if (level == 5)
        {
            cameraController.difficultyFactor = 1.8f;
        }
        else if (level > 5)
        {
            cameraController.difficultyFactor = (float)level / 3;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
