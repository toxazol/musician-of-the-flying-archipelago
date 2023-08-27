using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class FixedLooper : MonoBehaviour
{
    // Start is called before the first frame update

    // [SerializeField] private static int BPM = 120;
    // [SerializeField] private int maxPPM = (int) (60 * 60 / Time.fixedDeltaTime); // max pulse per minute
    [SerializeField] private int fpb = 8; // frames per beat // TODO: calculate 
    [SerializeField] private int bars = 4;
    [SerializeField] private int noteDivision = 8;
    [SerializeField] private int trackLen;
    [SerializeField] public struct Note
    {
        public bool isActive;
        public Toggle toggle;
    }
    [SerializeField] private Note[] notes;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int tickEvery = 0;
    [SerializeField] private bool isPause = false;
    [SerializeField] private bool isHighlighted = false;
    [SerializeField] private Color highColor;
    private int frame = 0;
    private int pulse = 0;

    private AudioSource audioSource;
    

    
    void Start()
    {
        trackLen = bars * noteDivision; 
        notes = new Note[trackLen];
        
        audioSource = GetComponent<AudioSource>();
        if(tickEvery > 0)
        {
            InitTicker(tickEvery);
        }
        InitGrid();
    }

    void InitTicker(int tickEvery)
    {
        for(int i = 0; i < trackLen; i++)
        {
            if(i%tickEvery != 0 ) continue;
            notes[i].isActive = true;
        }
    }

    void InitGrid()
    {
        int barLen = noteDivision;
        for(int i = 0; i < trackLen; i++)
        {
            int index = i;
            var cell = Instantiate(cellPrefab, this.transform);
            ColorizeCell(cell, i, barLen);
            var toggle = cell.GetComponent<Toggle>();
            notes[i].toggle = toggle;
            toggle.isOn = notes[i].isActive;
            toggle.onValueChanged.AddListener((val)=>{
                notes[index].isActive = val;
            });
        }
    }

    void ColorizeCell(GameObject cell, int i, int barLen)
    {
        if(i % barLen == 0)
            isHighlighted = !isHighlighted;
        if(!isHighlighted)
            return;
        cell.GetComponent<Image>().color = highColor;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if(isPause)
            return;
        if(pulse >= trackLen)
            pulse = 0;

        MoveCaret();

        if(frame % fpb == 0 && notes[pulse++].isActive)
            audioSource.Play();
        frame++;
    }

    void MoveCaret()
    {
        var curBtnPlayed = notes[pulse].toggle.transform.Find("Played").gameObject;
        curBtnPlayed.SetActive(true);
        var prevInd = pulse - 1 >= 0 ? pulse - 1 : trackLen - 1;
        var prevBtnPlayed = notes[prevInd].toggle.transform.Find("Played").gameObject;
        prevBtnPlayed.SetActive(false);
    }

    void OnToggleMusic()
    {
        isPause = !isPause;
    }

}
