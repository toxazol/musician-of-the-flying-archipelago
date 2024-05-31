using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action OnBlooperGuessed;
    public static void TriggerBlooperGuess()
    {
        OnBlooperGuessed?.Invoke(); 
    }
}