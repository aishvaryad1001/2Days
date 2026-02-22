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

    public GameObject[] allBtns;
    public GameObject resumeBtn;

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

        if (SaveManager.Instance.state.cards.Count > 0)
        {
            for (int i = 0; i < allBtns.Length; i++)
            {
                allBtns[i].SetActive(false);
            }
            resumeBtn.SetActive(true);
        }
        else
        {
            for (int i = 0; i < allBtns.Length; i++)
            {
                allBtns[i].SetActive(true);
            }
            resumeBtn.SetActive(false);
        }
    }

    public void OnClickResumeGame()
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

        if (open)
            settings.Play("Settings_Close");
        open = false;
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);

        AutoGridFit.instance.rows = SaveManager.Instance.state.row;
        AutoGridFit.instance.columns = SaveManager.Instance.state.column;

        AutoGridFit.instance.SetGrid();

        gameState = GameState.INGAME;
    }

    public void OnClickPlay_2x2()
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

        if (open)
            settings.Play("Settings_Close");
        open = false;
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 2;
        AutoGridFit.instance.columns = 2;
        SaveManager.Instance.state.row = AutoGridFit.instance.rows;
        SaveManager.Instance.state.column = AutoGridFit.instance.columns;
        AutoGridFit.instance.SetGrid();

        gameState = GameState.INGAME;
    }

    public void OnClickPlay_2x3()
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

        if (open)
            settings.Play("Settings_Close");
        open = false;
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 2;
        AutoGridFit.instance.columns = 3;
        SaveManager.Instance.state.row = AutoGridFit.instance.rows;
        SaveManager.Instance.state.column = AutoGridFit.instance.columns;
        AutoGridFit.instance.SetGrid();
        gameState = GameState.INGAME;
    }

    public void OnClickPlay_5x6()
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

        if (open)
            settings.Play("Settings_Close");
        open = false;
        mainMenuPanel.SetActive(false);
        InGameGUI.instance.inGamePanel.SetActive(true);
        AutoGridFit.instance.rows = 5;
        AutoGridFit.instance.columns = 6;
        SaveManager.Instance.state.row = AutoGridFit.instance.rows;
        SaveManager.Instance.state.column = AutoGridFit.instance.columns;
        AutoGridFit.instance.SetGrid();

        gameState = GameState.INGAME;
    }

    public void OnClickRestart()
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

        open = false;
        settings.Play("Settings_Close_Ingame");
        AutoGridFit.instance.SetGrid();

        gameState = GameState.INGAME;
    }

    public void OnClickSettings()
    {
        if (AutoGridFit.instance != null)
            if (AutoGridFit.instance.isCardMatching) return;

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