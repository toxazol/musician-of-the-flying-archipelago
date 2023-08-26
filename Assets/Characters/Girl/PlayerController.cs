using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Settings")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private ContactFilter2D moveFilter;
    [SerializeField] private float collisionOffset = 0.05f;
    [SerializeField] private Image hpUI;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private AttackZone attackZone;
    [SerializeField] private float invulnerabilityTime = 1f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private Health health;
    private SpriteRenderer spriteRenderer;
    private Blinking blinking;
    private List<RaycastHit2D> castCollisions = new();
    private int isInvulnerable = 0;
    private PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blinking = GetComponent<Blinking>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
        hpUI.fillAmount = health.GetHealthPercentage();
        EndAttack();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (TryMove(moveInput)
            || TryMove(new Vector2(moveInput.x, 0)) // for player not to get stuck
            || TryMove(new Vector2(0, moveInput.y))) // when moving diagonally
        {
            if (moveInput.x > 0)
            {
                MoveAnimation(true);
            }
            else
            {
                MoveAnimation(false);
            }
        }
        else
        {
            StopMoveAnimation();
        }
    }

    bool isTurnedLeft()
    {
        var state = animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName("idle_left")
               || state.IsName("run_left")
               || state.IsName("attack_left");
    }

    bool TryMove(Vector2 dir)
    {
        if (dir == Vector2.zero || animator.GetBool("isDead")) return false;

        int collisionCount = rb.Cast(
            dir,
            moveFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

        if (collisionCount > 0) return false;

        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * dir);
        return true;
    }

    private void MoveAnimation(bool isRight)
    {
        animator.SetBool("isMovingR", isRight);
        animator.SetBool("isMovingL", !isRight);
    }

    private void StopMoveAnimation()
    {
        animator.SetBool("isMovingR", false);
        animator.SetBool("isMovingL", false);
    }

    void OnMove(InputValue moveVal)
    {
        moveInput = moveVal.Get<Vector2>();
    }

    public void OnHit(int damage)
    {
        if (isInvulnerable > 0) return;

        StartInvulnerability();
        health.Hit(damage);
        hpUI.fillAmount = health.GetHealthPercentage();
        blinking.Blink();
        if (health.IsDead())
        {
            OnDie();
        }
    }

    public void OnDie()
    {
        Debug.Log("You are dead");
        animator.SetBool("isDead", true);
        animator.SetTrigger("dead");

        playerInput.enabled = false;
        levelManager.LoadDeathMenu();
    }

    void OnRespawn()
    {
        Debug.Log("Respawned");
        levelManager.ReloadCurentScene();
    }

    void OnFire()
    {
        attackZone.transform.localScale = new Vector3(isTurnedLeft() ? -1 : 1, 1, 1);
        animator.SetTrigger("Attack");
        Invoke("EndAttack", 0.3f);
    }

    private void StartInvulnerability()
    {
        isInvulnerable++;
        Invoke("CancelInvulnerability", invulnerabilityTime);
    }
    
    private void CancelInvulnerability()
    {
        isInvulnerable--;
    }

    private void EndAttack()
    {
        attackZone.transform.localScale = new Vector3(0, 0, 0);
    }
}