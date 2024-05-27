using System.Collections.Generic;
using UnityEngine;

public class FixedBlooper : MonoBehaviour
{
    [SerializeField] private FixedLooper targetLooper;

    [SerializeField] private int fpb = 8;
    [SerializeField] private int trackLen = 32;

    [SerializeField] private List<bool> correctNotes = new();
    
    [SerializeField] private bool isPause = false;
    
    
    private int frame = 0;
    private int pulse = 0;
    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        for (int i = correctNotes.Count; i < trackLen; i++) {
            correctNotes.Add(false);
        }
    }
 
    void FixedUpdate()
    {
        if(isPause)
            return;
        if(pulse >= trackLen)
            pulse = 0;

        if(frame % fpb == 0)
        {
            if(correctNotes[pulse])
                audioSource.Play();
            pulse++;
        }
            
        frame++;
    }


    void OnToggleMusic()
    {
        isPause = !isPause;
    }


    void OnEnable()
    {
        EventManager.OnNotePlay += OnNotePlay;
        targetLooper.OnNoteChange += OnNoteChange;
    }

    void OnDisable()
    {
        EventManager.OnNotePlay -= OnNotePlay;
        targetLooper.OnNoteChange -= OnNoteChange;
    }

    void OnNotePlay(string note)
    {
        Debug.Log("Blob heard " + note);
    }
    void OnNoteChange()
    {
        Debug.Log("Note changed");
        if(CompareMelody()) {
            Debug.Log("Melody guessed correctly!");
        }
    }

    bool CompareMelody() {
        for(int i = 0; i < targetLooper.notes.Length; i++) {
            if(targetLooper.notes[i].isActive != correctNotes[i])
                return false;
        }
        return true;
    }

}
