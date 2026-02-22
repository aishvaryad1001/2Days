using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameGUI : MonoBehaviour
{
    public static InGameGUI instance;
    public GameObject inGamePanel;

    public RectTransform layout;

    public TMP_Text scoreT;
    public int score = 0;

    public int tapCount = 0;
    public int cardsMatched = 0;

    public int currCombo = 0;
    public TMP_Text comboT;

    public Sprite[] cardsIcon;

    public FlipTheCard prevSelected;
    public GameObject fx;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        inGamePanel.SetActive(false);
        fx.SetActive(false);
        comboT.gameObject.SetActive(false);
    }

    public Vector3 CenterPosition()
    {
        return layout.rect.center;
    }

    public void UpdateScore(int _scoreToAdd)
    {
        score += _scoreToAdd;
        scoreT.text = score.ToString();
    }

    public void CheckIfAllCardsPaired()
    {
        cardsMatched--;
        if (cardsMatched <= 0)
        {
            MainMenuGUI.instance.gameState = GameState.GAMEOVER;
            GameOverGUI.instance.gameOverPanel.SetActive(true);
        }
    }
    Sequence seq;
    public void ShowComboFX()
    {
        if (currCombo > 0)
        {
            comboT.transform.localScale = Vector3.zero;

            comboT.gameObject.SetActive(true);
            if (currCombo > 1)
                comboT.text = "Combo " + currCombo;
            else
                comboT.text = "Combo";

            if (seq != null)
                seq.Kill();

            seq = DOTween.Sequence();
            seq.Join(comboT.transform.DOScale(Vector3.one * 1.1f, 0.35f).SetEase(Ease.OutBack));
            seq.AppendInterval(0.5f);
            seq.AppendCallback(() =>
            {
                comboT.gameObject.SetActive(false);
            });
        }
    }
}