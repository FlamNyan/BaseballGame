using UnityEngine;

public class BatCollisionHandler : MonoBehaviour
{
    // Drag your Bat_Handle GameObject into this slot in the Inspector
    [SerializeField] private BattingScript mainBatScript; 

    private void OnCollisionEnter(Collision collision)
    {
        // If we hit something and have a reference to the main script, pass it up!
        if (mainBatScript != null)
        {
            mainBatScript.ProcessHit(collision);
        }
    }
}