using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int currentHp = 100;
    [SerializeField] private int maxHp = 100;
    
    void Start()
    {
        
    }

    public int GetCurrentHp()
    {
        return currentHp;
    }
    
    public void Hit(int damage)
    {
        if (damage < 1) damage = 1;
        currentHp -= damage;
    }

    public bool IsDead()
    {
        return currentHp <= 0;
    }

    public float GetHealthPercentage()
    {
        return currentHp / (float)maxHp;
    }
    
    public int GetMaxHp()
    {
        return maxHp;
    }
}