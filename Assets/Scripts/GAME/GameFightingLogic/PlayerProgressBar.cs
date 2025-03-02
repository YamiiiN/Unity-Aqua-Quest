using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressBar : MonoBehaviour
{
    public Transform player; // Assign the player object
    public RectTransform pointer; // The moving UI pointer
    public float minMapX; // Minimum world X coordinate
    public float maxMapX; // Maximum world X coordinate
    public float minPointerX; // Minimum UI position for the pointer
    public float maxPointerX; // Maximum UI position for the pointer

    void Update()
    {
        if (player == null || pointer == null) return;

        // Clamp player's position within the min/max world coordinates
        float clampedX = Mathf.Clamp(player.position.x, minMapX, maxMapX);

        // Convert world position to normalized progress (0 to 1)
        float progress = Mathf.InverseLerp(minMapX, maxMapX, clampedX);

        // Map progress to UI pointer position
        float pointerX = Mathf.Lerp(minPointerX, maxPointerX, progress);

        // Update the pointer's anchored position (in local space)
        pointer.anchoredPosition = new Vector2(pointerX, pointer.anchoredPosition.y);
    }
}
