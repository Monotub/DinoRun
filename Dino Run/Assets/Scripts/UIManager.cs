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

    public void UpdateScore(int value)
    {
        scoreText.text = $"Score: {value}";
    }
}
