using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player
    public Vector3 offset = new Vector3(0, 2, -5); // Adjusted offset behind the player
    public float smoothSpeed = 5f; // Controls how smoothly the camera follows

    void LateUpdate()
    {
        if (target != null)
        {
            // Desired position
            Vector3 desiredPosition = target.position + offset;

            // Smooth transition to new position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Make camera look at the player
            transform.LookAt(target);
        }
    }
}
