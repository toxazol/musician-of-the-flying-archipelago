using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ContactFilter2D moveFilter;
    [SerializeField] private float collisionOffset = 0.05f;
    [SerializeField] private int currentHp = 100;
    [SerializeField] private int maxHp = 100;
    [SerializeField] private Image hpUI;
    
    private Vector2 moveInput; 
    private Rigidbody2D rb;
    private Animator animator;
    private List<RaycastHit2D> castCollisions = new();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        hpUI.fillAmount = currentHp / (float)maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() 
    {
        if(TryMove(moveInput)
        || TryMove(new Vector2(moveInput.x, 0))  // for player not to get stuck
        || TryMove(new Vector2(0, moveInput.y))) // when moving diagonally
        {
            if(moveInput.x > 0)
            {
                animator.SetBool("isMovingR", true);
                animator.SetBool("isMovingL", false);
            } else {
                animator.SetBool("isMovingR", false);
                animator.SetBool("isMovingL", true);
            }
        } else {
            animator.SetBool("isMovingR", false);
            animator.SetBool("isMovingL", false);
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

        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * dir);
        return true;
    }

    void OnMove(InputValue moveVal) 
    {
        moveInput = moveVal.Get<Vector2>();
    }

    public void OnHit(int damage)
    {
        currentHp -= damage;
        hpUI.fillAmount = currentHp / (float)maxHp;
        if(currentHp <= 0) 
        {
            Debug.Log("You are dead");
        }
    }

    void OnFire()
    {
        animator.SetTrigger("isAttack");
    }
}
