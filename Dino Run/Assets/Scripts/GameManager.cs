using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

    private void Update()
    {
        if (gamePaused) return;
        
        timer += Time.deltaTime;
        score = Mathf.RoundToInt(timer * 10);
        UIManager.Instance.UpdateScore(score);

        if (score > highscore) highscore = score;
        
    }

    public void StartGame()
    {
        gamePaused = false;
        UIManager.Instance.StartGame();
    }

    public void PauseGame()
    {
        gamePaused = true;
    }


}
