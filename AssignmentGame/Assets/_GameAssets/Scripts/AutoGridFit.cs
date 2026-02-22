using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class CardDetails
{
    public bool isMatched;
    public int cardIconIndex;
}

public class AutoGridFit : MonoBehaviour
{
    public static AutoGridFit instance;

    public bool isCardMatching = false;
    public bool isGridDisplayOver = false;

    public FlipTheCard gridItem;
    public int rows = 5;
    public int columns = 5;

    public Vector2 spacing = Vector2.zero;
    public RectOffset padding;

    public GridLayoutGroup grid;
    public RectTransform rectTransform;

    public List<FlipTheCard> allCards = new List<FlipTheCard>();
    public List<GameObject> matchedCards = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void SetGrid()
    {
        InGameGUI.instance.tapCount = 0;
        InGameGUI.instance.currCombo = -1;

        if (SaveManager.Instance.state.cards.Count > 0)
        {
            InGameGUI.instance.score = SaveManager.Instance.state.score;
            InGameGUI.instance.scoreT.text = InGameGUI.instance.score.ToString();

            InGameGUI.instance.cardsMatched = SaveManager.Instance.state.cards.Count;
            RebuildGrid();
            FitGrid();
        }
        else
        {

            SaveManager.Instance.state.cards.Clear();

            InGameGUI.instance.cardsMatched = allCards.Count;

            InGameGUI.instance.score = 0;
            InGameGUI.instance.scoreT.text = InGameGUI.instance.score.ToString();
            BuildGrid();
            FitGrid();

            if (matchedCards.Count > 0)
            {
                for (int i = 0; i < matchedCards.Count; i++)
                {
                    Destroy(matchedCards[i]);
                }
            }
        }
    }

    public void RebuildGrid()
    {
        for (int i = 0; i < SaveManager.Instance.state.cards.Count; i++)
        {
            FlipTheCard card = Instantiate(gridItem, transform);
            card.index = i;
            card.transform.localScale = Vector3.one;
            allCards.Add(card);
            card.cardIndex = SaveManager.Instance.state.cards[i].cardIconIndex;
            card.name = "Grid_" + card.cardIndex;

            if (SaveManager.Instance.state.cards[i].isMatched)
            {
                card.rect.gameObject.SetActive(false);
                InGameGUI.instance.cardsMatched--;
                matchedCards.Add(card.rect.gameObject);
                card.isFlipped = true;
            }
            card.item.sprite = InGameGUI.instance.cardsIcon[card.cardIndex];
            card.SetCardBase();
        }

        ShowFullGridOnStartOfGame();
    }

    public void BuildGrid()
    {
        // Clear old items
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        int totalItems = rows * columns;

        List<FlipTheCard> setCards = new List<FlipTheCard>();
        for (int i = 0; i < totalItems; i++)
        {
            FlipTheCard card = Instantiate(gridItem, transform);
            card.index = i;
            card.transform.localScale = Vector3.one;
            SaveManager.Instance.state.cards.Add(new CardDetails());
            card.SetCardBase();
            setCards.Add(card);
            allCards.Add(card);
        }

        int count = setCards.Count / 2;
        InGameGUI.instance.cardsMatched = setCards.Count;

        List<int> cardsIconList = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int chooseRandomCard = UnityEngine.Random.Range(0, setCards.Count);
            int index = UnityEngine.Random.Range(0, InGameGUI.instance.cardsIcon.Length);

            while (cardsIconList.Contains(index))
                index = UnityEngine.Random.Range(0, InGameGUI.instance.cardsIcon.Length);

            cardsIconList.Add(index);
            setCards[chooseRandomCard].item.sprite = InGameGUI.instance.cardsIcon[index];
            setCards[chooseRandomCard].cardIndex = index;
            setCards[chooseRandomCard].name = "Grid_" + index;
            SaveManager.Instance.state.cards[setCards[chooseRandomCard].index].cardIconIndex = index;
            SaveManager.Instance.state.cards[chooseRandomCard].isMatched = false;
            setCards.Remove(setCards[chooseRandomCard]);

            chooseRandomCard = UnityEngine.Random.Range(0, setCards.Count);
            setCards[chooseRandomCard].item.sprite = InGameGUI.instance.cardsIcon[index];
            setCards[chooseRandomCard].cardIndex = index;
            setCards[chooseRandomCard].name = "Grid_" + index;
            SaveManager.Instance.state.cards[setCards[chooseRandomCard].index].cardIconIndex = index;
            SaveManager.Instance.state.cards[chooseRandomCard].isMatched = false;
            setCards.Remove(setCards[chooseRandomCard]);
        }

        ShowFullGridOnStartOfGame();
    }

    public void ShowFullGridOnStartOfGame()
    {
        isGridDisplayOver = true;
        StartCoroutine(ShowFullGridOnStartOfGameCo());
    }

    IEnumerator ShowFullGridOnStartOfGameCo()
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            if (!SaveManager.Instance.state.cards[i].isMatched)
            {
                allCards[i].FlipCard();
            }
        }
        var delay = allCards.Count * 0.1f;
        if (delay <= 1)
            delay = 1;

        yield return new WaitForSeconds(delay);

        for (int i = 0; i < allCards.Count; i++)
        {
            if (!SaveManager.Instance.state.cards[i].isMatched)
            {
                allCards[i].CloseTheCard();
            }
        }

        isGridDisplayOver = false;
    }

    void FitGrid()
    {
        if (rows <= 0 || columns <= 0) return;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;
        grid.spacing = spacing;
        grid.padding = padding;
        grid.childAlignment = TextAnchor.MiddleCenter;

        float availableWidth = rectTransform.rect.width - padding.left - padding.right - spacing.x * (columns - 1);

        float availableHeight = rectTransform.rect.height - padding.top - padding.bottom - spacing.y * (rows - 1);

        float cellSize = Mathf.Min(availableWidth / columns, availableHeight / rows);

        grid.cellSize = new Vector2(cellSize, cellSize);
    }

    void OnRectTransformDimensionsChange()
    {
        if (!gameObject.activeInHierarchy) return;
        FitGrid();
    }
}