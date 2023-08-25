using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
    [SerializeField] private int attackDamage = 5;

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
        if (collider.gameObject.TryGetComponent(out Enemy enemy))
        {
            Debug.Log("Attacked " + enemy.GetEnemyName());
            enemy.SetKnockback(5000f);
            enemy.OnHit(attackDamage);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        detectedObjs.Remove(other);
    }
}
