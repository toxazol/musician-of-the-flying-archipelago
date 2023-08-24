using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private DetectionZone detectionZone;
    [SerializeField] private float moveForce = 500f;
    [SerializeField] private int maxHp = 50;
    [SerializeField] private int attackDamage = 5;


    Animator animator;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if(detectionZone.detectedObjs.Count == 0)
        {
            animator.SetBool("isMoving", false);
            return;
        }

        animator.SetBool("isMoving", true);

        var dir = (detectionZone.detectedObjs[0].transform.position - transform.position).normalized;

        rb.AddForce(moveForce * Time.fixedDeltaTime * dir);

    }

    

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("test");
        if(collision.gameObject.tag != "Player") return;
        IDamageable damageableObj = collision.collider.GetComponent<IDamageable>();
        damageableObj.OnHit(attackDamage);
    }

    public void OnHit(int damage)
    {
        Debug.Log("enemy damaged");
    }
}
