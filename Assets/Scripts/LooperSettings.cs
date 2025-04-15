using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LooperSettings : MonoBehaviour
{
    [field: SerializeField] public int Fpb {get; private set;} = 8; // frames per beat 
    [field: SerializeField] public int Bars {get; private set;} = 4;
    [field: SerializeField] public int NoteDivision {get; private set;} = 8;
    [field: SerializeField] public int TrackLen {get; private set;} = 0;
    [field: SerializeField] public bool IsHighlighted {get; private set;} = false;
    [field: SerializeField] public Color HighColor {get; private set;}
    [field: SerializeField] public int UserFrameDelta {get; set;} = -7;
    [field: SerializeField] public int UserFrameTolerance {get; private set;} = 3;
    [field: SerializeField] public float IndicationSecs {get; private set;} = 0.3f;
    [field: SerializeField] public float BuffSecs {get; private set;} = 0.3f;
    [field: SerializeField] public GameObject HitIndicator {get; private set;}
    [field: SerializeField] public AttackZone PlayerAttack {get; private set;}
    [field: SerializeField] public int DamageRhythm {get; private set;} = 20;
    [field: SerializeField] public int DamageDefault {get; private set;} = 2;
    [field: SerializeField] public float KnockBackRhythm {get; private set;} = 6000f;
    [field: SerializeField] public float KnockBackDefault {get; private set;} = 1000f;

}
