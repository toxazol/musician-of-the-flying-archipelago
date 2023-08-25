using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Note 
{
    public AudioClip sound;
    public double time;
    public Note(AudioClip sound, double time)
    {
        this.sound = sound;
        this.time = time;
    }
}

[System.Serializable]
public struct Tick 
{
    public AudioClip tic;
    public AudioClip tac;
}


public class Caret : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float trackLen = 812f;
    [SerializeField] private int BPM = 120;
    [SerializeField] private int barCount = 4;
    [SerializeField] private int ticksPerBar = 4;

    [SerializeField] private List<Note> melody = new();    
    [SerializeField] private Tick tick;
    [SerializeField] private double tolerance = 0.125f;
    

    private bool isStarted = false;
    private Vector3 startingPosition;
    private double trackTime; // track length in seconds
    private int loops = 0; // loops counter
    private float caretSpeed;
    private double startPlayTime; 
    private int closestNoteInd = -1;

    private AudioSource audioSource;
    

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        startingPosition = transform.position;
        trackTime = barCount * ticksPerBar * 60f / BPM;
        caretSpeed = (float)(trackLen / trackTime);

        InitTicker();
    }

    void Update() // TODO: -> FixedUpdate?
    {
        if(!isStarted)
        {
            return;
        }

        CheckRestart();

        CheckMelody();

        transform.Translate(caretSpeed * Time.deltaTime, 0, 0);

        CheckInput();
    }

    public void StartPlay()
    {
        isStarted = true;
        startPlayTime = AudioSettings.dspTime;
        // audioSource.PlayOneShot(tick.tac); // TODO: only if ticker enabled
        // StartCoroutine(PlaySoundAtInterval(GetComponent<AudioClip>(), 60f / BPM));
        // StartCoroutine(StartWith4Ticks());
    }

    void InitTicker()
    {
        double startTime = 0;
        for(int i = 0; i < barCount * ticksPerBar; i++)
        {
            double tickTime = startTime + i * 60f / BPM;
            AudioClip nextTick = i % 4 == 0 ? tick.tac : tick.tic;
            Note note = new(nextTick, tickTime);
            melody.Add(note);
        }
    }

    void CheckMelody() 
    {
        if(closestNoteInd == -1)
        {
            //find closest note
            closestNoteInd = melody.FindIndex(0, melody.Count-1, note => note.time > now);
        }
        if(closestNoteInd < 0 || closestNoteInd >= melody.Count)
        {
            Debug.Log("closestNoteInd="+ closestNoteInd + " is out of bounds");
            return;
        }

        Note closestNote = melody[closestNoteInd];
        if(getLoopTime() + tolerance > closestNote.time)
        {
            double realNoteTime = closestNote.time + loops * trackTime;
            audioSource.clip = closestNote.sound; 
            audioSource.PlayScheduled(realNoteTime);
            closestNoteInd++;
        }
    
    }

    private double getLoopTime()
    {
        return AudioSettings.dspTime - startPlayTime - loops * trackTime;
    }

    void CheckRestart()
    {
        if(Mathf.Abs(transform.position.x - startingPosition.x) > trackLen)
        {
            transform.position = startingPosition;
        }
        if(getLoopTime() + tolerance > trackTime)
        {
            closestNoteInd = 0;
            loops++;
        }
    }

}
