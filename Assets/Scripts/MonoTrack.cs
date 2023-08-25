using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Note 
{
    public float xCoord;
    public double time;
    public Note(float xCoord, double time)
    {
        this.xCoord = xCoord;
        this.time = time;
    }
}


public class Caret : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float trackLen = 812f;
    [SerializeField] private int BPM = 120;
    [SerializeField] private int barCount = 4;
    [SerializeField] private int ticksPerBar = 4;

    [SerializeField] private KeyCode keyCode;    
    [SerializeField] private List<Note> melody = new();    
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
    
    void CheckInput() 
    {
        if(Input.GetKeyDown(keyCode))
        {
            AddNote();
        }
    }

    void AddNote()
    {
        audioSource.Play();
        
        melody.Add(new Note(transform.position.x, getLoopTime()));
        
        melody.Sort((a,b) => a.time < b.time);

        //draw note
    }

}
