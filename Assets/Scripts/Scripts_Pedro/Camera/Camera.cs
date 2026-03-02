using UnityEngine;

public class CameraGulosa : MonoBehaviour
{
    public Transform player;
    public float velocidade = 0.1f;
    public float cameraZ = -10f;
    public Vector2 offset;

    [Header("Camera Bounds")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    private void Start()
    {
        cam = GetComponent<Camera>();

        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            cameraZ
        );

        float clampedX = Mathf.Clamp(
            targetPos.x,
            minBounds.x + halfWidth,
            maxBounds.x - halfWidth
        );

        float clampedY = Mathf.Clamp(
            targetPos.y,
            minBounds.y + halfHeight,
            maxBounds.y - halfHeight
        );

        Vector3 clampedPosition = new Vector3(clampedX, clampedY, cameraZ);

        transform.position = Vector3.Lerp(transform.position, clampedPosition, velocidade);
    }
}