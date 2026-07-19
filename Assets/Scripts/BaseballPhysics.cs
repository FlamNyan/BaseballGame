using UnityEngine;
using BaseballGame.Scripts.Managers; 

[RequireComponent(typeof(Rigidbody))]
public class BaseballPhysics : MonoBehaviour
{
    [Header("Flight Data (Provided by Manager)")]
    private float currentSpeedMPS;
    private float currentSpinRateRPM;
    private Vector3 currentSpinAxis;

    [Header("Aerodynamic Constants (Real-World Values)")]
    [Tooltip("Air density at sea level in kg/m^3")]
    [SerializeField] private float airDensity = 1.225f; 
    
    [Tooltip("Cross-sectional area of a standard baseball in m^2")]
    [SerializeField] private float ballArea = 0.0042f;  
    
    [Tooltip("Drag coefficient of a standard baseball")]
    [SerializeField] private float dragCoefficient = 0.35f; 

    [Header("Gameplay Tuning")]
    [Tooltip("Adjust this to exaggerate or reduce the physical break of the pitch.")]
    [SerializeField] private float magnusScale = 0.005f; 

    // Components
    private Rigidbody rb;
    private bool isInFlight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0f; 
        rb.angularDamping = 0.05f;
    }

    public void InitializePitch(PitchData data)
    {
        currentSpeedMPS = data.SpeedMPS;
        currentSpinRateRPM = data.SpinRateRPM;
        currentSpinAxis = data.SpinAxis;

        rb.linearVelocity = transform.forward * currentSpeedMPS;
        isInFlight = true;
    }

    private void FixedUpdate()
    {
        if (!isInFlight) return;

        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        // Scenario Account: Stop calculating if the ball hits the catcher's mitt or ground
        if (speed < 0.1f) 
        {
            isInFlight = false;
            return; 
        }

        // --- 1. DRAG FORCE ---
        // Formula: F_d = 0.5 * rho * v^2 * C_d * A
        float dragMagnitude = 0.5f * airDensity * (speed * speed) * dragCoefficient * ballArea;
        Vector3 dragDirection = -velocity.normalized;
        Vector3 dragForce = dragDirection * dragMagnitude;

        // --- 2. MAGNUS FORCE (SPIN BREAK) ---
        // Convert RPM to Radians per Second for mathematical accuracy
        float spinRadSec = currentSpinRateRPM * (Mathf.PI / 30f); 
        Vector3 spinVector = currentSpinAxis * spinRadSec;

        // The cross product finds the exact perpendicular direction the air pushes the ball
        Vector3 magnusDirection = Vector3.Cross(spinVector, velocity).normalized;
        
        // Calculate the magnitude of the break. 
        // We multiply by our magnusScale so you can balance the game's difficulty.
        float magnusMagnitude = spinRadSec * speed * airDensity * ballArea * magnusScale;
        Vector3 magnusForce = magnusDirection * magnusMagnitude;

        // --- 3. APPLY FORCES ---
        rb.AddForce(dragForce + magnusForce, ForceMode.Force);
    }

    
}