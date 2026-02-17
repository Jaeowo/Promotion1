using UnityEngine;

public class HeadAttackWave : MonoBehaviour
{
    BoxCollider2D headAttackCollider;
    private float speed = 4.5f;
    Vector2 newSize;

    private void Awake()
    {
        headAttackCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        headAttackCollider.size = new Vector2(1.8f, 0.8f);
        newSize = headAttackCollider.size;
    }
    private void Update()
    {
        newSize.x += speed * Time.deltaTime;
        headAttackCollider.size = newSize;
    }

}
