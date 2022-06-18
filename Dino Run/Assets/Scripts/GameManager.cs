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
    float timer = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        gamePaused = true;
        LoadScores();
    }

    private void Start()
    {
    }

    private void Update()
    {
        UIManager.Instance.UpdateScore(score, highscore);
        
        if (gamePaused || dino.IsDead) return;
        
        timer += Time.deltaTime;
        score = Mathf.RoundToInt(timer * 10);

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
        gamePaused = false;
        UIManager.Instance.StartGame();
        dino.GetComponent<Animator>().Play("Run");

    }

    public void PauseGame()
    {
        gamePaused = true;
    }

    public void RestartScene()
    {
        SaveScores();
        SceneManager.LoadScene(0);
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
