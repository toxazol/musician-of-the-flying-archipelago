using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ysort : MonoBehaviour
{
    [SerializeField] private int sortingOrderBase = 5000;
    // [SerializeField] private float bottomOffset = 0f;
    [SerializeField] private int precisionMul = 100;
    [SerializeField] private bool onceOnly = true;
    
    private Renderer myRenderer;
    private Transform myTransform;
    private BoxCollider2D box2D;
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myTransform = GetComponent<Transform>();
        box2D = GetComponent<BoxCollider2D>();
    }

    void LateUpdate()
    {
        float colliderBottomOffsetY = box2D.offset.y > 0 ? 
            box2D.offset.y + box2D.size.y/2 : box2D.offset.y - box2D.size.y/2;

        myRenderer.sortingOrder = (int) (sortingOrderBase 
            - (myTransform.position.y + colliderBottomOffsetY) * precisionMul);
        if (onceOnly)
        {
            Destroy(this);
        }
    }
}
