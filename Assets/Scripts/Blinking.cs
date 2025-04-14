using UnityEngine;

public class Blinking : MonoBehaviour
{
    [SerializeField] private Color blinkColor = new Color(1f, 1f, 1f, 0f);
    [SerializeField] private int timesOfBLinking = 2;
    [SerializeField] private float blinkingSpeed = 0.1f;
    
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Blink()
    {
        Blink(timesOfBLinking, blinkingSpeed);
    }
    
    public void Blink(bool recover)
    {
        Blink(timesOfBLinking, blinkingSpeed, recover);
    }
    
    public void Blink(int times, float speed)
    {
        Blink(times, speed, true);
    }
    
    public void Blink(int times, float speed, bool recover)
    {
        float time = 0f;
        for (int i = 0; i < times; i++)
        {
            Invoke("EnableBlink", time);
            time += speed;
            if (recover)
            {
                Invoke("DisableBlink", time);
                time += speed;
            }
        }
    }

    private void EnableBlink()
    {
        spriteRenderer.color = blinkColor;
    }

    private void DisableBlink()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}