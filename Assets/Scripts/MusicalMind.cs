using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalMind : MonoBehaviour
{

    [SerializeField] private List<FixedBlooper> fixedBloopers = new List<FixedBlooper>();
    private bool isPaused = true;

    [ContextMenu("Sing")]
    void Sing()
    {
        isPaused = !isPaused;
        foreach(FixedBlooper fixedBlooper in fixedBloopers) {
            fixedBlooper.CompareMelody();
            fixedBlooper.isPause = isPaused;
        }
    }

    void OnEnable()
    {
        EventManager.OnBlooperGuessed += OnBlooperGuessed;
    }

    void OnDisable()
    {
        EventManager.OnBlooperGuessed -= OnBlooperGuessed;
    }

    void OnBlooperGuessed()
    {
        Debug.Log("OnBlooperGuess");
        foreach(FixedBlooper fixedBlooper in fixedBloopers) {
            if(!fixedBlooper.isGuessed) return;
        }
        Debug.Log("MELODY GUESSED CORRECTLY");
    }
}
