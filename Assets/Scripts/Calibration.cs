using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Calibration : MonoBehaviour
{
    [SerializeField] private int userFrameDelta = 0;

    [SerializeField] private int fpb = 8; // frames per beat 
    [SerializeField] private int noteDivision = 8;
    [SerializeField] private int trackLen;
    [SerializeField] private int tickEvery = 4;
    [SerializeField] private bool isPause = false;
    [SerializeField] private LooperSettings targetSettings;
    
    [SerializeField] private bool[] notes;
    
    private int frame = 0;
    private int pulse = 0;
    private int lastBeatFrame = 0;
    private AudioSource audioSource;
    

    void Start()
    {
        trackLen = targetSettings.Bars * noteDivision; 
        notes = new bool[trackLen];
        
        audioSource = GetComponent<AudioSource>();
        if(tickEvery > 0)
        {
            InitTicker(tickEvery);
        }
    }

    void InitTicker(int tickEvery)
    {
        for(int i = 0; i < trackLen; i++)
        {
            if(i%tickEvery != 0 ) continue;
            notes[i] = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isPause)
            return;
        if(pulse >= trackLen)
            pulse = 0;

        if(frame % fpb == 0 && notes[pulse++])
        {
            audioSource.Play();
            lastBeatFrame = frame;
        }
        frame++;
    }


    void OnFire()
    {
        int fireFrame = frame;
        int prevDelta = fireFrame - lastBeatFrame;
        int nextBeatFrame = lastBeatFrame + fpb * tickEvery;
        int nextDelta = nextBeatFrame - fireFrame;
        userFrameDelta = nextDelta < prevDelta ? -1 * nextDelta : prevDelta;
        targetSettings.UserFrameDelta = userFrameDelta;
    }

}
