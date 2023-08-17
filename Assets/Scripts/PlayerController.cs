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
    private List<RaycastHit2D> castCollisions = new();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        if(TryMove(moveInput)) return;
        TryMove(new Vector2(moveInput.x, 0));
        TryMove(new Vector2(0, moveInput.y)); // yes
        // TryMove(moveInput);
    }

    bool TryMove(Vector2 dir) 
    {
        if(dir == Vector2.zero) return false;
        
        int collisionCount = rb.Cast(
            dir, 
            moveFilter, 
            castCollisions, 
            moveSpeed * Time.fixedDeltaTime + collisionOffset);
        
        if(collisionCount > 0) return false;

        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * dir);
        return true;
    }

    void OnMove(InputValue moveVal) 
    {
        moveInput = moveVal.Get<Vector2>();
    }
}
