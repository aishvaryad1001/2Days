using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoGridFit : MonoBehaviour
{
    public static AutoGridFit instance;

    public GameObject gridItem;
    public int rows = 5;
    public int columns = 5;

    public Vector2 spacing = Vector2.zero;
    public RectOffset padding;

    public GridLayoutGroup grid;
    public RectTransform rectTransform;


    void Start()
    {
        BuildGrid();
        FitGrid();
    }

    public void BuildGrid()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        int totalItems = rows * columns;

        for (int i = 0; i < totalItems; i++)
        {
            GameObject card = Instantiate(gridItem, transform);
            card.transform.localScale = Vector3.one;
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