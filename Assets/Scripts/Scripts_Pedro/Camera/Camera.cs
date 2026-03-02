using UnityEngine;

public class CameraGulosa : MonoBehaviour
{
    public Transform player;
    public float velocidade = 0.1f;
    public float cameraZ = -10f;
    public Vector2 offset;

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 targetPos = new Vector3(
            player.position.x + offset.x,
            player.position.y + offset.y,
            cameraZ
        );

        transform.position = Vector3.Lerp(transform.position, targetPos, velocidade);
    }
}