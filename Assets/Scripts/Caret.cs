using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Note 
{
    public GameObject note;
    public KeyCode keyCode;
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
    [SerializeField] private float pixelsInTrack = 812f;
    [SerializeField] private int BPM = 120;
    [SerializeField] private int barCount = 4;
    [SerializeField] private int ticksPerBar = 4;

    [SerializeField] private Note[] notes;    
    // [SerializeField] private Tick tick;
    

    private bool isStarted = false;
    private Vector3 startingPosition;
    // private Dictionary<double, AudioSource> noteTimeStamps = new();
    private float trackTime;
    private float caretSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStarted)
        {
            return;
        }

        CheckRestart();

        trackTime = barCount * ticksPerBar * 60f / BPM;
        caretSpeed = pixelsInTrack / trackTime;
        transform.Translate(caretSpeed * Time.deltaTime, 0, 0);

        CheckInput();
    }

    public void UnPause()
    {
        // isStarted = true;
        // StartCoroutine(PlaySoundAtInterval(GetComponent<AudioSource>(), 60f / BPM));
        StartCoroutine(StartWith4Ticks());
    }

    void CheckRestart()
    {
        if(Mathf.Abs(transform.position.x - startingPosition.x) > pixelsInTrack)
        {
            transform.position = startingPosition;
        }
    }
    
    void CheckInput() 
    {
        foreach (var note in notes)
        {
            if(Input.GetKeyDown(note.keyCode))
            {
                AddNote(note);
            }
        }
    }

    void AddNote(Note note)
    {
        var noteInstance = Instantiate(note.note, transform.parent.transform, false);
        noteInstance.transform.Translate(transform.position.x - noteInstance.transform.position.x, 0, 0);

        StartCoroutine(PlaySoundAtInterval(noteInstance.GetComponent<AudioSource>(), trackTime));
    }

    private IEnumerator PlaySoundAtInterval(AudioSource sound, float interval)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            sound.Play();
        }
    }

    private IEnumerator StartWith4Ticks()
    {
        for (int i=0; i<ticksPerBar; i++)
        {
            yield return new WaitForSeconds(60f / BPM);
            GetComponent<AudioSource>().Play();
        }
        isStarted = true;
    }

}
