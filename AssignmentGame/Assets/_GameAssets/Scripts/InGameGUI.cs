using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameGUI : MonoBehaviour
{
    public static InGameGUI instance;

    public RectTransform layout;

    public Sprite[] cardsIcon;

    private void Awake()
    {
        instance = this;
    }

    public Vector3 CenterPosition()
    {
        return layout.rect.center;
    }
}