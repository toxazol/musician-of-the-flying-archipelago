using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Note 
{
    public GameObject note;
    public KeyCode keyCode;
}

public class Caret : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxPatrolDistance = 812f;
    [SerializeField] private float stepDistance = 1f;
    [SerializeField] private float stepsPerSecond = 5f;

    [SerializeField] private Note[] notes;    
    

    private Vector3 startingPosition;
    private int playingNoteId;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRestart();
        transform.position += new Vector3(stepDistance * stepsPerSecond * Time.deltaTime, 0, 0);
        CheckNoteCollision();
        CheckInput();
    }

    void CheckRestart()
    {
        if(Mathf.Abs(transform.position.x - startingPosition.x) > maxPatrolDistance)
        {
            transform.position = startingPosition;
            playingNoteId = 0;
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
        playingNoteId = noteInstance.GetInstanceID();
    }

    void CheckNoteCollision()
    {
        var hitNoteColliders = new List<Collider2D>();
        var filter = new ContactFilter2D();
        filter.NoFilter();
        var collisions = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hitNoteColliders);
        if(collisions > 0)
        {
            var hitNote = hitNoteColliders[0].gameObject;
            if(playingNoteId == hitNote.GetInstanceID())
            {
                return;
            }
            playingNoteId = hitNote.GetInstanceID();
            hitNote.GetComponent<AudioSource>().Play();
        }
    }
}
