using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private GameObject harmonoid;
    [SerializeField] private AudioClip punch;
    [SerializeField] private AudioClip stepL;
    [SerializeField] private AudioClip stepR;

    private bool isAnimationBlocked;
    private bool isRun;
    private bool isAttack = false;
    private int attackAnimationCounter = 0;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;
    private Health health;
    private Blinking blinking;
    private List<RaycastHit2D> castCollisions = new();
    private int isInvulnerable = 0;
    private PlayerInput playerInput;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        animator = GetComponent<Animator>();
        blinking = GetComponent<Blinking>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = true;
        hpUI.fillAmount = health.GetHealthPercentage();
        attackZone.transform.localScale = new Vector3(0, 0, 0);
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
            isRun = true;
            PlayBlockable("run");
            animator.SetFloat("xMove", moveInput.x);
            animator.SetFloat("yMove", moveInput.y);
        }
        else
        {
            PlayBlockable("idle");
            isRun = false;
        }
    }

    void ActivateDamageZone()
    {
        float x = animator.GetFloat("xMove");
        float y = animator.GetFloat("yMove");
        Debug.Log(y);
        if (y != 0)
        {
            attackZone.transform.Rotate(y < 0 ? 160 : 0, 0, 90);
            attackZone.transform.localPosition = new Vector3(0.75f, y < 0 ? 0 : 0.5f, 0);
        }
        else
        {
            attackZone.transform.localRotation = new Quaternion(0, 0, 0, 1);
            attackZone.transform.localPosition = new Vector3(0, 0, 0);
        }

        attackZone.transform.localScale = new Vector3(x < 0 ? -1 : 1, 1, 1);
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

    void OnMove(InputValue moveVal)
    {
        moveInput = moveVal.Get<Vector2>();
    }

    private void PlayBlockable(string name)
    {
        if (attackAnimationCounter == 0)
        {
            isAnimationBlocked = false;
        }
        if (!isAnimationBlocked)
        {
            animator.Play(name);
        }
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
        levelManager.ReloadCurentScene();
    }

    void OnFire()
    {
        if (attackAnimationCounter == 0)
        {
            isAnimationBlocked = false;
        }
        
        if (!isAnimationBlocked)
        {
            ActivateDamageZone();
            isAttack = true;
            if (isRun)
            {
                PlayBlockable("attack_on_move");
            }
            else
            {
                PlayBlockable("attack");
            }

            isAnimationBlocked = true;
            attackAnimationCounter++;
            Invoke("DeacreaceAttackCounter", 1f);
        }
    }

    private void DeacreaceAttackCounter()
    {
        attackAnimationCounter--;
    }

    public void OnAttackAnimationExit()
    {
        attackZone.transform.localRotation = new Quaternion(0, 0, 0, 1);
        attackZone.transform.localScale = new Vector3(0, 0, 0);
        isAnimationBlocked = false;
        isAttack = false;
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

    public float GetDistanceToHarmonoid()
    {
        return Vector3.Distance(harmonoid.transform.position, this.GameObject().transform.position);
    }

    public void PlayPunch()
    {
        Debug.Log("punched");
        audioSource.PlayOneShot(punch);
    }
}