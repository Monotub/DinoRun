using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioMixerGroup sfxMixer;
    [SerializeField] AudioMixerGroup musicMixer;
    [Header("Clips")]
    [SerializeField] AudioClip[] footSounds;
    [SerializeField] AudioClip dinoRoar;
    [SerializeField] AudioClip dinoJump;
    [SerializeField] AudioClip bodyThud;


    public static AudioManager Instance;
    
    int numOfAudioManagers;
    public bool musicMuted { get; private set; }
    public bool sfxMuted { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        numOfAudioManagers = FindObjectsOfType<AudioManager>().Length;

        if (numOfAudioManagers > 1)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        Player.PlayFootstepSFX += PlayFootSounds;
        Player.PlayDeathSounds += PlayDeathSounds;
        Player.OnDinoRoar += PlayDinoRoar;
    }

    private void OnDisable()
    {
        Player.PlayFootstepSFX -= PlayFootSounds;
        Player.PlayDeathSounds -= PlayDeathSounds;
        Player.OnDinoRoar -= PlayDinoRoar;
    }

    private void PlayFootSounds()
    {
        int rand = UnityEngine.Random.Range(0, footSounds.Length - 1);

        sfx.PlayOneShot(footSounds[rand]);
    }

    private void PlayDeathSounds()
    {
        sfx.PlayOneShot(bodyThud);
    }

    void PlayDinoRoar()
    {
        sfx.PlayOneShot(dinoRoar);
    }

    public void PlayDinoJump()
    {
        sfx.PlayOneShot(dinoJump);
    }

    public void ToggleMusic()
    {
        if (!musicMuted)
        {
            musicMixer.audioMixer.SetFloat("MusicVol", -80);
            musicMuted = true;
        }
        else
        {
            musicMixer.audioMixer.SetFloat("MusicVol", 0);
            musicMuted = false;
        }

        UIManager.Instance.ToggleMusicButton();
    }

    public void ToggleSFX()
    {
        if (!sfxMuted)
        {
            sfxMixer.audioMixer.SetFloat("SFXVol", -80);
            sfxMuted = true;
        }
        else
        {
            sfxMixer.audioMixer.SetFloat("SFXVol", 0);
            sfxMuted = false;
        }
        UIManager.Instance.ToggleSFXButton();
    }
}
