using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IKnockbackable
{
    [SerializeField] private DetectionZone detectionZone;
    [SerializeField] private float moveForce = 500f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private string enemyName = "blob";
    [SerializeField] private float knockbackResist = 0f;
    [SerializeField] private int armor = 0;

    Blinking blinking;
    Animator animator;
    Rigidbody2D rb;
    
    private HealthBar healthBar;
    private Health health;
    private DamageDisplay damageDisplay;

    private float knockbackPower;
    private int knockbackTicks = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        blinking = GetComponent<Blinking>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        healthBar = GetComponentInChildren<HealthBar>();
        damageDisplay = GetComponentInChildren<DamageDisplay>();
        healthBar.Invoke("setFullHp", 0f);
    }

    public string GetEnemyName()
    {
        return enemyName;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        if (animator.IsDestroyed()) return;
        if (detectionZone.detectedObjs.Count == 0)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        animator.SetBool("isMoving", true);

        if (knockbackPower > 0f && knockbackTicks < 10)
        {
            var dir = (-(detectionZone.detectedObjs[0].transform.position - transform.position).normalized);
            rb.AddForce((knockbackPower - knockbackResist) * Time.fixedDeltaTime * dir);
            knockbackTicks++;
        }
        else
        {
            var dir = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;
            rb.AddForce(moveForce * Time.fixedDeltaTime * dir);
            knockbackTicks = 0;
            knockbackPower = 0;
        }
    }

    public void OnKnockback(float knockbackPower)
    {
        this.knockbackPower = knockbackPower;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        IDamageable damageableObj = collision.collider.GetComponent<IDamageable>();
        damageableObj.OnHit(attackDamage);
    }

    public void OnHit(int damage)
    {
        var calculatedDamage = damage - armor/2;
        health.Hit(calculatedDamage);
        damageDisplay.AddDamage(calculatedDamage);
        healthBar.updateHp(health);

        if (health.IsDead())
        {
            OnDie();
        }
        else
        {
            blinking.Blink();
        }
    }

    public void OnDie()
    {
        blinking.Blink(false);
        Destroy(rb);
        Destroy(animator);
        Destroy(detectionZone);
        Destroy(GetComponent<Ysort>());
        Invoke("DestroyEnemy", 0.7f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}