using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    NONE,
    MAIN_MENU,
    INGAME,
    GAMEOVER
}

public class MainMenuGUI : MonoBehaviour
{
    public static MainMenuGUI instance;

    public GameState gameState;
    public GameObject mainMenuPanel;

    public Animator settings;
    public bool open = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameState = GameState.MAIN_MENU;
        mainMenuPanel.SetActive(true);
        settings.gameObject.SetActive(true);
    }

    public void OnClickPlay_2x2()
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

        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 2;
        AutoGridFit.instance.columns = 2;

        gameState = GameState.INGAME;
    }

    public void OnClickPlay_2x3()
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
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 2;
        AutoGridFit.instance.columns = 3;

        gameState = GameState.INGAME;
    }

    public void OnClickPlay_5x6()
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
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 5;
        AutoGridFit.instance.columns = 6;

        gameState = GameState.INGAME;
    }

    public void OnClickRestart()
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

        open = false;
        settings.Play("Settings_Close_Ingame");
        AutoGridFit.instance.SetGrid();

        gameState = GameState.INGAME;
    }

    public void OnClickSettings()
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

        open = !open;

        if (open)
        {
            if (gameState == GameState.MAIN_MENU)
                settings.Play("Settings_Open");
            else if (gameState == GameState.INGAME)
                settings.Play("Settings_Open_Ingame");
        }
        else
        {
            if (gameState == GameState.MAIN_MENU)
                settings.Play("Settings_Close");
            else if (gameState == GameState.INGAME)
                settings.Play("Settings_Close_Ingame");
        }
    }
}