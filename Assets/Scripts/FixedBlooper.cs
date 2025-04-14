using System.Collections.Generic;
using UnityEngine;

public class FixedBlooper : MonoBehaviour
{
    [SerializeField] private FixedLooper targetLooper;

    [SerializeField] private int fpb = 8;
    [SerializeField] private int trackLen = 32;

    [SerializeField] private List<bool> correctNotes = new();
    
    public bool isPause = true;

    public bool isGuessed = false;
    
    
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

    void OnEnable()
    {
        targetLooper.OnNoteChange += OnNoteChange;
    }

    void OnDisable()
    {
        targetLooper.OnNoteChange -= OnNoteChange;
    }

    void OnNoteChange()
    {
        // CompareMelody();
    }

    public void CompareMelody() {
        for(int i = 0; i < targetLooper.notes.Length; i++) {
            if(targetLooper.notes[i].isActive != correctNotes[i]){
                isGuessed = false;
                return;
            }
        }
        // Debug.Log("Blooper " + this.gameObject.name + "'s melody guessed correctly!");
        isGuessed = true;
        EventManager.TriggerBlooperGuess();
    }

}
