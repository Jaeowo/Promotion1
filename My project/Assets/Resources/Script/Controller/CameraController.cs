using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Transform playerTransform;

    [Header("Transform")]
    [SerializeField] private Vector3 cameraPosition;
    [SerializeField] private Vector2 center;
    [SerializeField] private Vector2 mapSize;

    [Header("Setting")]
    [SerializeField] private float cameraMoveSpeed;
    private float height;
    private float width;

    private void Awake()
    {
        playerTransform = player.GetComponent<Transform>();
    }

    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        LimitCameraArea();
    }

    private void LimitCameraArea()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraPosition,
                                          Time.deltaTime * cameraMoveSpeed);

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(center, mapSize * 2);
    }

}
