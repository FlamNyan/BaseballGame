using UnityEngine;

public class PitchCamera : MonoBehaviour
{
    [Header("Tracking Settings")]
    [Tooltip("Drag the Baseball prefab from the Hierarchy here.")]
    [SerializeField] private Transform targetBall;
    
    // We store the camera's original position so it doesn't move, it just rotates.
    private Vector3 startingPosition;

    private void Start()
    {
        // Remember where the camera was placed in the Inspector
        startingPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (targetBall != null)
        {
            // Lock the camera to its starting position (like standing on the mound)
            transform.position = startingPosition;
            
            // Track the ball with rotation only
            transform.LookAt(targetBall);
        }
    }
}