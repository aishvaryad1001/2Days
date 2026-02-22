using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoGridFit : MonoBehaviour
{
    public static AutoGridFit instance;

    public FlipTheCard gridItem;
    public int rows = 5;
    public int columns = 5;

    public Vector2 spacing = Vector2.zero;
    public RectOffset padding;

    public GridLayoutGroup grid;
    public RectTransform rectTransform;

    public List<FlipTheCard> allCards = new List<FlipTheCard>();

    void Start()
    {
        BuildGrid();
        FitGrid();
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
            card.transform.localScale = Vector3.one;
            setCards.Add(card);
            allCards.Add(card);
        }

        int count = setCards.Count / 2;
        List<int> cardsIconList = new List<int>();

        for (int i = 0; i < count; i++)
        {
            int chooseRandomCard = Random.Range(0, setCards.Count);
            int index = Random.Range(0, InGameGUI.instance.cardsIcon.Length);

            while (cardsIconList.Contains(index))
                index = Random.Range(0, InGameGUI.instance.cardsIcon.Length);

            cardsIconList.Add(index);
            setCards[chooseRandomCard].item.sprite = InGameGUI.instance.cardsIcon[index];
            setCards[chooseRandomCard].cardIndex = index;
            setCards[chooseRandomCard].name = "Grid_" + index;
            setCards.Remove(setCards[chooseRandomCard]);

            chooseRandomCard = Random.Range(0, setCards.Count);
            setCards[chooseRandomCard].item.sprite = InGameGUI.instance.cardsIcon[index];
            setCards[chooseRandomCard].cardIndex = index;
            setCards[chooseRandomCard].name = "Grid_" + index;
            setCards.Remove(setCards[chooseRandomCard]);
        }
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