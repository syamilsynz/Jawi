using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{

    public AudioSource musicPlayer;
    public AudioSource effectPlayer;
    public AudioSource effectGame;
 
    public AudioClip menuClickClip;
    public AudioClip letterClickClip;
    public AudioClip correctAnswerClip;
    public AudioClip wrongAnswerClip;
    public AudioClip cashRegisterClip;

    public bool audioEnabled;

    static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () 
    {

        if (SaveManager.audioEnabled == 1)
        {
            audioEnabled = true;

            if (musicPlayer)
                musicPlayer.Play();
        }
        else
        {
            audioEnabled = false;
        }
    }

    public void ChangeAudioState()
    {
        if (audioEnabled)
        {
            audioEnabled = false;
            SaveManager.audioEnabled = 0;

            if (musicPlayer)
                musicPlayer.Stop();

            if (effectPlayer)
                effectPlayer.Stop();
        }
        else
        {
            audioEnabled = true;
            SaveManager.audioEnabled = 1;

            if (musicPlayer)
                musicPlayer.Play();
        }

        SaveManager.SaveData();
    }

    public void PlayMenuClick()
    {
        if (menuClickClip && audioEnabled)
        {
            effectPlayer.clip = menuClickClip;
            effectPlayer.Play();
        }
    }
    public void PlayLetterClick()
    {
        if (letterClickClip && audioEnabled)
        {
            effectPlayer.clip = letterClickClip;
            effectPlayer.Play();
        }
    }       
    public void PlayCashRegister()
    {
        if (cashRegisterClip && audioEnabled)
        {
            effectPlayer.clip = cashRegisterClip;
            effectPlayer.Play();

        }
    }
    public void PlayCorrectAnswer()
    {
        if (correctAnswerClip && audioEnabled)
        {
            effectPlayer.clip = correctAnswerClip;
            effectPlayer.Play();

        }
    }
    public void PlayWrongAnswer()
    {
        if (wrongAnswerClip && audioEnabled)
        {
            effectPlayer.clip = wrongAnswerClip;
            effectPlayer.Play();

        }
    }
}
