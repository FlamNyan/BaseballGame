using UnityEngine;

public class BattingScript : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask cubeLayerMask; // Set this to 'TargetCube' in Inspector
    [SerializeField] private float speed = 20f;       // How fast the bat tracks the mouse

    void Update()
    {
        // Fires a ray from the camera to the mouse position.
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        // 2. checks if the ray fired hits the cube (the cube layer)
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, cubeLayerMask))
        {
            // Get the exact 3D point where the mouse is hovering on the cube
            Vector3 targetPoint = raycastHit.point;

            // 3. Calculate the direction from the bat to that point
            Vector3 targetDirection = targetPoint - transform.position;

            // 4. Calculate the rotation needed to look at that direction
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // 5. Smoothly rotate the bat towards the target point
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
        }
    }
}