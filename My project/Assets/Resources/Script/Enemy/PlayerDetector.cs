using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] float detectionAngle = 60f; // Cone in front of enemy
    [SerializeField] float detectionRadius = 10f; // Large circle around enemy
    [SerializeField] float innerDetectionRadius = 5f; // Small circle around enemy
    [SerializeField] float detectionCooldown = 1f; // Time between detections
    [SerializeField] float attackRange = 2f; // Distance from enemy to player to attack

    public Transform player { get; private set; }
    CountdownTimer detectionTimer;


    IDetectionStrategy detectionStrategy;

    private void Start()
    {
        detectionTimer = new CountdownTimer(detectionCooldown);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() => detectionTimer.Tick(Time.deltaTime);

    public bool CanDetectPlayer()
    {
        return detectionTimer.IsRunning || detectionStrategy.Execute(player, transform, detectionTimer);
    }

    public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;
}
