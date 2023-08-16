using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private ContactFilter2D moveFilter;
    [SerializeField] private float collisionOffset = 0.05f;


    private Vector2 moveInput; 
    private Rigidbody2D rb;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(moveInput == Vector2.zero)
        {
            return;
        }
        int count = rb.Cast(
            moveInput, 
            moveFilter, 
            castCollisions, 
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
        
        if(count > 0) {
            return;
        }
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void OnMove(InputValue moveVal) {
        moveInput = moveVal.Get<Vector2>();
    }
}
