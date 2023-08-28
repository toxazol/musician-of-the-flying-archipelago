using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FriendController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Rigidbody2D friend;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float followDistance = 2f;
    [SerializeField] private ContactFilter2D moveFilter;
    [SerializeField] private float collisionOffset = 0.05f;
    [SerializeField] private bool isFollow = true;
    [SerializeField] private Image gui;
    [SerializeField] private AudioClip stepL;
    [SerializeField] private AudioClip stepR;
    private AudioSource audioSource;

    private Vector2 moveInput; 
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new();
    private bool isStepLeft = false;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Vector2.zero;
        if(!isFollow) return;

        Vector2 friendDir = friend.position - rb.position;
        if(friendDir.magnitude > followDistance)
        {
            moveInput = friendDir.normalized;
        }
    }

    private void FixedUpdate() 
    {
        if(TryMove(moveInput)
        || TryMove(new Vector2(moveInput.x, 0))  // for player not to get stuck
        || TryMove(new Vector2(0, moveInput.y))) // when moving diagonally
        {
            animator.SetBool("isMoving", true);
            if(moveInput.x > 0)
            {
                sr.flipX = false;
            } else {
                sr.flipX = true;
            }
        } else {
            animator.SetBool("isMoving", false);
        }
         
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

        if(!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(isStepLeft ? stepL : stepR);
            isStepLeft = !isStepLeft;
        }
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * dir);
        return true;
    }

    public void OnWhistle()
    {
        isFollow = !isFollow;
        gui.color = isFollow ? Color.white : Color.black;
    }

}
