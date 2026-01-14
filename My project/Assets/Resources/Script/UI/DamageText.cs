using UnityEngine;

public class DamageText : MonoBehaviour
{
    private StopwatchTimer validTimer;

    private Vector2 impulseRangeX = new Vector2(-5.3f, 5.3f);
    private Vector2 impulseRangeY = new Vector2(4.5f, 5.5f);
    private float gravity = -4f;
    private float damping = 0.92f;

    private Vector3 velocity;

    private void Start()
    {
        validTimer = new StopwatchTimer();
        validTimer.Reset();
        validTimer.Start();

        float x = Random.Range(-0.5f, 0.5f);
        float y = Random.Range(0.8f, 1.2f);

        float dirX = Random.value < 0.5f ? -1f : 1f;

        velocity = new Vector3(
            dirX * Random.Range(impulseRangeX.x * -1f, impulseRangeX.y),
            Random.Range(impulseRangeY.x, impulseRangeY.y),
            0f);
    }

    private void Update()
    {
        velocity.y += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        velocity *= damping;

        validTimer.Tick(Time.deltaTime);

        if (validTimer.GetTime() >= 1f)
        {
            Destroy(gameObject);
        }
    }

}
