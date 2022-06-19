using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player dino;

    public bool gamePaused {get; private set;}
    public static GameManager Instance;

    int score = 0;
    int highscore;
    float scoreTimer = 0;
    int numOfGameManagers;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        numOfGameManagers = FindObjectsOfType<GameManager>().Length;

        if (numOfGameManagers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);

        gamePaused = true;
        LoadScores();
    }

    private void Update()
    {
        UIManager.Instance.UpdateScore(score, highscore);
        
        if (gamePaused || dino.IsDead) return;
        
        scoreTimer += Time.deltaTime;
        score = Mathf.RoundToInt(scoreTimer * 10);

        if (score > highscore)
            highscore = score;
    }

    void LoadScores()
    {
        if (PlayerPrefs.HasKey("HighScore"))
            highscore = PlayerPrefs.GetInt("HighScore");
        else 
            highscore = 0;
    }

    void SaveScores()
    {
        PlayerPrefs.SetInt("HighScore", highscore);
        PlayerPrefs.Save();
    }

    public void StartGame()
    {
        if (dino == null) dino = FindObjectOfType<Player>();

        gamePaused = false;
        score = 0;
        UIManager.Instance.StartGame();
        dino.GetComponent<Animator>().Play("Run");
    }

    public void PauseGame()
    {
        gamePaused = true;
    }

    /// <summary>
    /// Referenced by UI Restart button
    /// </summary>
    public void RestartScene()
    {
        score = 0;
        scoreTimer = 0;
        GameManager.Instance.SaveScores();
        SceneManager.LoadScene(0);
        UIManager.Instance.RestartGame();
        UIManager.Instance.UpdateScore(score, highscore);
    }

    private void OnApplicationQuit()
    {
        SaveScores();
    }

    public void ResetHighscore()
    {
        highscore = 0;
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }
}
