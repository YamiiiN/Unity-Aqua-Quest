using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public float smoothSpeed = 5f; // Smooth camera movement speed
    private float yOffset = 8.082069f; // Keep Y position fixed
    private float zOffset = -4.98f; // Camera depth position

    private float minX = -8.54f; // Left boundary
    private float maxX = 53.88f; // Right boundary

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player")?.transform;

            if (target == null)
            {
                Debug.LogError("CameraFollow: No Player found in scene!");
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        // Get the target position and clamp within minX and maxX
        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 targetPosition = new Vector3(clampedX, yOffset, zOffset);

        // Smoothly move the camera to the new position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
