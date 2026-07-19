using UnityEngine;

public class BattingScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 15f; 
    [SerializeField] private float aimingDepth = 10f; 

    // Adjust this in the Inspector to fix the offset (e.g., 90, -90, 180)
    [SerializeField] private float rotationOffsetOffsetY = 0f; 

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
                // 1. Get the standard, stable camera-aligned look rotation
                Quaternion lookRotation = Quaternion.LookRotation(targetDirection, mainCamera.transform.up);

                // 2. Apply a clean Y-axis offset to fix your model's crooked artwork
                Quaternion targetRotation = lookRotation * Quaternion.Euler(0, rotationOffsetOffsetY, 0);

                // 3. Smoothly blend to it
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
        }
    }
}