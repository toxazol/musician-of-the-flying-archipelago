using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FixedLooper : MonoBehaviour
{
    // Start is called before the first frame update

    // [SerializeField] private static int BPM = 120;
    // [SerializeField] private int maxPPM = (int) (60 * 60 / Time.fixedDeltaTime); // max pulse per minute
    [SerializeField] private static int fpb = 4; // frames per beat // TODO: calculate 
    [SerializeField] private static int bars = 4;
    [SerializeField] private static int noteDivision = 4;
    [SerializeField] private static int trackLen = bars * noteDivision; // 4 bars of 16th
    [SerializeField] private bool[] notes = new bool[trackLen];
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private bool isTick = false;
    private int frame = 0;
    private int pulse = 0;

    private AudioSource audioSource;
    

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(isTick)
        {
            InitTicker();
        }
        InitGrid();
    }

    void InitTicker()
    {
        for(int i = 0; i < trackLen; i++)
        {
            if(i%4 != 0 ) continue;
            notes[i] = true;
        }
    }

    void InitGrid()
    {
        for(int i = 0; i < trackLen; i++)
        {
            int index = i;
            var cell = Instantiate(cellPrefab, this.transform);
            var btn = cell.GetComponent<Toggle>();

            btn.isOn = notes[i];
            btn.onValueChanged.AddListener((val)=>{
                notes[index] = val;
            });
        }
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if(pulse >= trackLen)
        {
            pulse = 0;
        }
        if(frame % fpb == 0 && notes[pulse++])
        {
            audioSource.Play();
        }
        frame++;
    }

}
