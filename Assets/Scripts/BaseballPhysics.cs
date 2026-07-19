using UnityEngine;
using BaseballGame.Scripts.Managers; // Allows access to PitchData

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

    // Components
    private Rigidbody rb;
    private bool isInFlight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // We use custom drag calculations in flight, so disable Unity's default drag
        rb.linearDamping = 0f; 
        rb.angularDamping = 0f;
    }

    // Called by the throw mechanism to pass the Manager's data into the ball.
    public void InitializePitch(PitchData data)
    {
        currentSpeedMPS = data.SpeedMPS;
        currentSpinRateRPM = data.SpinRateRPM;
        currentSpinAxis = data.SpinAxis;

        // Apply the initial forward burst of speed (Assuming pitching down Z axis)
        rb.linearVelocity = transform.forward * currentSpeedMPS;
        
        isInFlight = true;
    }
    
}
