using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Vector3 offset; // Offset position from the player

    private void LateUpdate()
    {
        if (player != null)
        {
            // Update the camera's position based on the player's position + offset
            transform.position = player.position + offset;
            transform.LookAt(player);
        }
    }
}
