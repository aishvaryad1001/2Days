using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class SaveState
{
    public bool isSound = true;
    public bool isVibration = true;

    public int score = 0;
    public int row = 0;
    public int column = 0;
    public List<CardDetails> cards = new List<CardDetails>();
}