using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    private int iterations = 0;
    private int stackedDamage = 0;
    private TextMeshProUGUI text;
    private Animator animator;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    public void AddDamage(int damage)
    {
        stackedDamage += damage;
        text.text = stackedDamage + "";
        iterations++;

        DisplayDamage(true);
        animator.SetTrigger("Combo");
        Invoke("DecreaceCounter", 0.8f);
    }

    private void DecreaceCounter()
    {
        iterations--;
        if (iterations < 1)
        {
            Invoke("ResetStackedDamage", 0.05f);
            DisplayDamage(false);
        }
    }

    private void ResetStackedDamage()
    {
        if (iterations < 1)
        {
            stackedDamage = 0;
        }
    }

    private void DisplayDamage(bool display)
    {
        // text.enabled = display;
        animator.SetBool("Display", display);
    }
}