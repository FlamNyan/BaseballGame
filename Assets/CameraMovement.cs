using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{   
    public GameObject secondCamera;
    private Camera mainCamera;
    public Transform targetPosition;
    public float duration = 3.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(MoveCameraRoutine(targetPosition.position));
    }

    IEnumerator MoveCameraRoutine(Vector3 endPos)
    {
        float timeElapsed = 0;
        Vector3 startPos = transform.position;

        while (timeElapsed < duration)
        {
            // The t value represents a percentage of time completed
            float t = timeElapsed / duration;
            
            // Optional: apply Mathf.SmoothStep(0.0f, 1.0f, t) here for ease-in/out
            
            transform.position = Vector3.Lerp(startPos, endPos, t);
            
            timeElapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure we reach the exact final position at the end
        transform.position = endPos;
        secondCamera.SetActive(true);
    }
} 
