using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverGUI : MonoBehaviour
{
    public static GameOverGUI instance;

    public GameObject gameOverPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnClickHome()
    {
        if (SoundManager.instance.isSoundOn)
        {
            SoundManager.instance.gameSound.clip = SoundManager.instance.click;
            SoundManager.instance.gameSound.Play();
        }

        if (SoundManager.instance.isVibratrionOn)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        SceneManager.LoadScene(0);
    }
}