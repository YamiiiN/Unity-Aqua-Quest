using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public float smoothSpeed = 5f; // Smooth camera movement speed
    public float yOffset = 0f; // Keep Y position fixed
    public float zOffset = -10f; // Keep the camera at a proper depth

    public float minX = -5f; // Left boundary
    public float maxX = 50f; // Right boundary

    private void LateUpdate()
    {
        if (target == null) return;

        // Get the target position and clamp it within minX and maxX
        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        Vector3 targetPosition = new Vector3(clampedX, yOffset, zOffset);

        // Smoothly move the camera to the new position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
