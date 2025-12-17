using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private float checkRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }

    void Update()
    {
        IsGrounded = Physics2D.OverlapCircle(checkPoint.position, checkRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (checkPoint == null) 
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(checkPoint.position, checkRadius);
    }
}
