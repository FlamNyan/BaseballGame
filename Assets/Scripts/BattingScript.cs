using UnityEngine;

public class BattingScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 15f; 
    [SerializeField] private float aimingDepth = 10f; 

    // Adjust this in the Inspector to fix the offset (e.g., 90, -90, 180)
    [SerializeField] private float rotationOffsetOffsetY = 0f; 

    [Header("Pull Back Settings")]
    [Tooltip("How much the bat pulls back based on mouse distance.")]
    [SerializeField] private float pullbackIntensity = 5f; 
    [Tooltip("The maximum angle (in degrees) the bat can pull back.")]
    [SerializeField] private float maxPullbackAngle = 35f;

    [Header("Physics Impact Settings")]
    [Tooltip("The amount of force applied to the ball upon impact.")]
    [SerializeField] private float hitForce = 20f;
    [Tooltip("Adds an upward lift vector so the ball goes airborne like a pop-fly.")]
    [SerializeField] private float upwardLift = 0.5f;

    void Update()
    {
        Vector3 planeCenter = transform.position + mainCamera.transform.forward * aimingDepth;
        Plane aimingPlane = new Plane(-mainCamera.transform.forward, planeCenter);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (aimingPlane.Raycast(ray, out float distanceToPlane))
        {
            Vector3 targetPoint = ray.GetPoint(distanceToPlane);
            Vector3 targetDirection = targetPoint - transform.position;

            if (targetDirection != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection, mainCamera.transform.up);

                float distance = targetDirection.magnitude; 
                float pullbackX = Mathf.Min(distance * pullbackIntensity, maxPullbackAngle);

                Quaternion targetRotation = lookRotation * Quaternion.Euler(pullbackX, rotationOffsetOffsetY, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
        }
    }

    // Public method to process the hit data sent from the child object (Player_Bat)
    public void ProcessHit(Collision collision)
    {
        // Check if the object we hit has a Rigidbody component to apply force to
        Rigidbody targetRigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (targetRigidbody != null)
        {
            // Calculate a launch direction based on where the bat is facing, plus an upward lift factor
            Vector3 launchDirection = transform.forward + (Vector3.up * upwardLift);
            launchDirection.Normalize(); // Normalize keeps the force consistent regardless of the direction math

            // Apply the physics push instantly using Impulse mode
            targetRigidbody.AddForce(launchDirection * hitForce, ForceMode.Impulse);
            
            Debug.Log($"Whack! Hit {collision.gameObject.name} with a force of {hitForce} via passthrough!");
        }
    }
}