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

    public bool isSoundOn = false;
    public bool isVibratrionOn = false;

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
        if (PlayerPrefs.HasKey("SOUND"))
        {
            isSoundOn = bool.Parse(PlayerPrefs.GetString("SOUND"));
        }
        else
        {
            PlayerPrefs.SetString("SOUND", true.ToString());
            isSoundOn = true;
        }

        if (PlayerPrefs.HasKey("VIBRATIONS"))
        {
            isVibratrionOn = bool.Parse(PlayerPrefs.GetString("VIBRATIONS"));
        }
        else
        {
            PlayerPrefs.SetString("VIBRATIONS", true.ToString());
            isVibratrionOn = true;
        }

        Setup();
    }

    public void Setup()
    {
        if (isSoundOn)
            soundImg.sprite = soundOn;
        else
            soundImg.sprite = soundOff;

        if (isVibratrionOn)
            vibrationImg.sprite = vibrationOn;
        else
            vibrationImg.sprite = vibrationOff;
    }

    public void OnClickSound()
    {
        if (isSoundOn)
        {
            gameSound.clip = click;
            gameSound.Play();
        }

        if (isVibratrionOn)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        isSoundOn = !isSoundOn;
        if (isSoundOn)
            soundImg.sprite = soundOn;
        else
            soundImg.sprite = soundOff;

        PlayerPrefs.SetString("SOUND", isSoundOn.ToString());
    }

    public void OnClickVibration()
    {
        if (isSoundOn)
        {
            gameSound.clip = click;
            gameSound.Play();
        }

        if (isVibratrionOn)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        isVibratrionOn = !isVibratrionOn;
        if (isVibratrionOn)
            vibrationImg.sprite = vibrationOn;
        else
            vibrationImg.sprite = vibrationOff;
        PlayerPrefs.SetString("VIBRATIONS", isVibratrionOn.ToString());
    }
}