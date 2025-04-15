using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [field: SerializeField] public int AttackDamage { get; set; } = 1;
    [field: SerializeField] public float KnockbackPower { get; set; } = 5000f;
    [SerializeField] private bool isPlayerSource = true;

    public List<Collider2D> detectedObjs = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("Enemy") && !isPlayerSource) return; // enemy don't kill each other
        if (collider.gameObject.tag.Equals("Friend") && isPlayerSource) return; // no friendly fire
        if (collider.gameObject.tag.Equals("Player") && isPlayerSource) return; // no self harm
        
        if (collider.gameObject.TryGetComponent(out IDamageable damageable))
        {
            damageable.OnHit(AttackDamage);
        }
        if (collider.gameObject.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.OnKnockback(KnockbackPower);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        detectedObjs.Remove(other);
    }
}
