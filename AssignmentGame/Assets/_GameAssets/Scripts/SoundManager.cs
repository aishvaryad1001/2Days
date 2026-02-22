using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Image soundImg;
    public Sprite soundOn;
    public Sprite soundOff;

    public Image vibrationImg;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    public AudioSource gameSound;

    public AudioClip click;
    public AudioClip flipping;
    public AudioClip wrongMatch;
    public AudioClip rightMatch;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        if (SaveManager.Instance.state.isSound)
            soundImg.sprite = soundOn;
        else
            soundImg.sprite = soundOff;

        if (SaveManager.Instance.state.isVibration)
            vibrationImg.sprite = vibrationOn;
        else
            vibrationImg.sprite = vibrationOff;
    }

    public void OnClickSound()
    {
        if (SaveManager.Instance.state.isSound)
        {
            gameSound.clip = click;
            gameSound.Play();
        }

        if (SaveManager.Instance.state.isVibration)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        SaveManager.Instance.state.isSound = !SaveManager.Instance.state.isSound;
        if (SaveManager.Instance.state.isSound)
            soundImg.sprite = soundOn;
        else
            soundImg.sprite = soundOff;

    }

    public void OnClickVibration()
    {
        if (SaveManager.Instance.state.isSound)
        {
            gameSound.clip = click;
            gameSound.Play();
        }

        if (SaveManager.Instance.state.isVibration)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        SaveManager.Instance.state.isVibration = !SaveManager.Instance.state.isVibration;
        if (SaveManager.Instance.state.isVibration)
            vibrationImg.sprite = vibrationOn;
        else
            vibrationImg.sprite = vibrationOff;
    }

    private void OnApplicationFocus(bool focus)
    {
        SaveManager.Instance.UpdateState();
    }
}