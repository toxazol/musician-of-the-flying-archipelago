using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private string targetTag = "Player";

    public List<Collider2D> detectedObjs = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag != targetTag) return;
        detectedObjs.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        detectedObjs.Remove(other);
    }
}
