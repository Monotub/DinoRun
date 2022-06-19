using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Canvas startScreen;
    [SerializeField] Canvas restartScreen;
    [SerializeField] TMP_Text tempInstructions;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;
    [SerializeField] Button musicButton;
    [SerializeField] Button sfxButton;
    [SerializeField] Button restartButton;


    [Header("Animators")]
    [SerializeField] Animator startAnim;
    [SerializeField] Animator scoreAnim;

    public static UIManager Instance;
    int numOfUIManagers;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        numOfUIManagers = FindObjectsOfType<UIManager>().Length;

        if (numOfUIManagers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        RestartGame();
    }

    public void StartGame()
    {
        startAnim.SetTrigger("Close");
        tempInstructions.color = new Color(1, 1, 1, 0.3f);
    }

    public void RestartGame()
    {
        startScreen.enabled = true;
        restartScreen.enabled = false;
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

    public void ToggleSFXButton()
    {
        if (AudioManager.Instance.sfxMuted)
            sfxButton.GetComponent<Image>().color = Color.red;
        else
            sfxButton.GetComponent<Image>().color = Color.white;
    }

    public void ToggleMusicButton()
    {
        if (AudioManager.Instance.musicMuted)
            musicButton.GetComponent<Image>().color = Color.red;
        else
            musicButton.GetComponent<Image>().color = Color.white;
    }
}
