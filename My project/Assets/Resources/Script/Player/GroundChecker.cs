using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [SerializeField] float checkRadius = 0.1f;
    [SerializeField] Vector3 pivot = Vector3.zero;
    [SerializeField] LayerMask groundLayers;

    public bool IsGrounded { get; private set; }

    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(transform.position + pivot, checkRadius, groundLayers);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + pivot, checkRadius);
    }
}
