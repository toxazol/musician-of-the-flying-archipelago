using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private RectTransform rect;
    private Canvas canvas;
   
    void Start()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    
    public void updateHp(Health health)
    {
        rect.sizeDelta = new Vector2(health.GetHealthPercentage(), 0.85f);
        canvas.enabled = health.GetMaxHp() != health.GetCurrentHp();
    }

}