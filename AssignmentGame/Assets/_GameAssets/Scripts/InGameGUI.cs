using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameGUI : MonoBehaviour
{
    public static InGameGUI instance;

    public RectTransform layout;

    public int tapCount = 0;
    public Sprite[] cardsIcon;

    public FlipTheCard prevSelected;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 CenterPosition()
    {
        return layout.rect.center;
    }
}