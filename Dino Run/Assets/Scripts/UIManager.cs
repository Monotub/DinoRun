using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Canvas startScreen;
    [SerializeField] Canvas restartScreen;
    [SerializeField] TMP_Text tempInstructions;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    [Header("Animators")]
    [SerializeField] Animator startAnim;
    [SerializeField] Animator scoreAnim;

    public static UIManager Instance;


    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        restartScreen.enabled = false;
        scoreText.text = "Score: 0";
    }

    public void StartGame()
    {
        startAnim.SetTrigger("Close");
        tempInstructions.color = new Color(1, 1, 1, 0.3f);
    }

    public void ShowRestartUI()
    {
        restartScreen.enabled = true;
    }

    public void UpdateScore(int score, int highscore)
    {
        scoreText.text = $"Score: {score.ToString("n0")}";
        highScoreText.text = highscore.ToString("n0");
    }
}
