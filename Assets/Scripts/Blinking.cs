using UnityEngine;

public class Blinking : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Blink(int times, float speed)
    {
        float time = 0f;
        for (int i = 0; i < times; i++)
        {
            Invoke("EnableBlink", time);
            time += speed;
            Invoke("DisableBlink", time);
            time += speed;
        }
    }

    private void EnableBlink()
    {
        spriteRenderer.color = new Color(255f, 255f, 255f, 0f);
    }

    private void DisableBlink()
    {
        spriteRenderer.color = new Color(255f, 255f, 255f, 255f);
    }
}