using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooperSettings : MonoBehaviour
{
    public int fpb = 8; // frames per beat 
    public int bars = 4;
    public int noteDivision = 8;
    public int trackLen;
    public bool isHighlighted = false;
    public Color highColor;
    public int userFrameDelta = -16;
    public int userFrameTolerance = 2;
    public float indicationSecs = 0.3f;
    public GameObject hitIndicator;

}
