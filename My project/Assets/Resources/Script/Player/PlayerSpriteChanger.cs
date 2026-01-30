using UnityEngine;

public class PlayerSpriteChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BaseStat playerStat;

    private Color originalColor;
    private float blinkSpeed = 2f;
    private bool wasInvincible = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerStat = GetComponent<BaseStat>();

        originalColor = spriteRenderer.color;
    }

    private void Update()
    {
        ActiveChecker();
    }

    private void ActiveChecker()
    {
        if (playerStat.IsInvincible)
        {
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            spriteRenderer.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            wasInvincible = true;
        }
        else
        {
            if (wasInvincible)
            {
                spriteRenderer.color = originalColor;
                wasInvincible = false;
            }
        }
    }
}
