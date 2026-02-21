using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipTheCard : MonoBehaviour
{
    public Image bg;
    public Image item;
    
    private void Start()
    {
        bg.gameObject.SetActive(true);
        item.gameObject.SetActive(false);
    }
}