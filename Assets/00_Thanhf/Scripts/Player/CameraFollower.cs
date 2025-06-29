using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform target; // The target to follow
    [SerializeField] private Vector3 offset; // Offset from the target position
    [SerializeField] private float smoothSpeed = 0.125f; // Smoothing speed

    void LateUpdate()
    {
        if (target == null) return;

        // Calculate the desired position
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate to the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        // Update the camera position
        transform.position = smoothedPosition;
    }
}
