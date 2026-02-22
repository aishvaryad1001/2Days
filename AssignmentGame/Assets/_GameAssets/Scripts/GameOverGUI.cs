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
        if (SaveManager.Instance.state.isSound)
        {
            SoundManager.instance.gameSound.clip = SoundManager.instance.click;
            SoundManager.instance.gameSound.Play();
        }

        if (SaveManager.Instance.state.isVibration)
        {
            Vibration.Init();
            Vibration.VibratePop();
        }

        SceneManager.LoadScene(0);
    }
}