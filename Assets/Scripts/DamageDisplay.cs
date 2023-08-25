using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    private int iterations = 0;
    private int stackedDamage = 0;
    private TextMeshProUGUI text;
    
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void AddDamage(int damage)
    {
        stackedDamage += damage;
        text.text = stackedDamage + "";
        iterations++;
        
        DisplayDamage(true);
        Invoke("DecreaceCounter", 1f);
    }

    private void DecreaceCounter()
    {
        iterations--;
        if (iterations < 1)
        {
            stackedDamage = 0;
            DisplayDamage(false);
        }
    }

    private void DisplayDamage(bool display)
    {
        text.enabled = display;
    }
}