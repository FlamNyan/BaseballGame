using UnityEngine;

public class BattingScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float speed = 15f; // Increased for tighter, crisper control

    void Update()
    {
        // 1. Create an invisible mathematical plane passing through the bat
        // and facing directly towards the camera so it is perfectly flat.
        Plane aimingPlane = new Plane(-mainCamera.transform.forward, transform.position);

        // 2. Fire a ray from the mouse
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // 3. Find exactly where the ray intersects our flat invisible wall
        if (aimingPlane.Raycast(ray, out float distanceToPlane))
        {
            // Get the clean 3D coordinate on that flat surface
            Vector3 targetPoint = ray.GetPoint(distanceToPlane);

            // 4. Calculate the direction from the bat to that point
            Vector3 targetDirection = targetPoint - transform.position;

            if (targetDirection != Vector3.zero)
            {
                // 5. Calculate the crisp rotation
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

                // 6. Smoothly blend to it
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            }
        }
    }
}