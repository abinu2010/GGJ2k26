using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public Transform player;
    public float xOffset = 5.0f;
    private float maxXPosition;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in WallMovement script.");
            // Try to find the player by tag if not assigned
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                enabled = false; // Disable the script if player is not found
                return;
            }
        }
        maxXPosition = transform.position.x;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            float targetX = player.position.x - xOffset;
            if (targetX > maxXPosition)
            {
                maxXPosition = targetX;
                transform.position = new Vector3(maxXPosition, transform.position.y, transform.position.z);
            }
        }
    }
}
