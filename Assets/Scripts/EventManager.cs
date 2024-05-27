using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action<string> OnNotePlay;

    public static void TriggerNotePlay(string note)
    {
        OnNotePlay?.Invoke(note);
    }
}