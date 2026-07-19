using UnityEngine;
using BaseballGame.Scripts.Managers; 

[RequireComponent(typeof(Rigidbody))]
public class BaseballPhysics : MonoBehaviour
{
    [Header("Flight Data (Provided by Manager)")]
    private PitchType currentPitchType;
    private float currentSpeedMPS;
    private float currentSpinRateRPM;
    private Vector3 currentSpinAxis;

    [Header("Aerodynamic Constants")]
    [SerializeField] private float airDensity = 1.225f; 
    [SerializeField] private float ballArea = 0.0042f;  
    [SerializeField] private float dragCoefficient = 0.35f; 

    [Header("Gameplay Tuning")]
    [SerializeField] private float magnusScale = 0.005f; 
    
    [Tooltip("How fast the knuckleball darts around")]
    [SerializeField] private float flutterFrequency = 15f; 
    [Tooltip("How hard the knuckleball gets pushed sideways/up/down")]
    [SerializeField] private float flutterStrength = 1.5f;

    private Rigidbody rb;
    private TrailRenderer trailRenderer; 
    private bool isInFlight = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        trailRenderer = GetComponent<TrailRenderer>(); 
        rb.linearDamping = 0f; 
        rb.angularDamping = 0.05f;
    }

    public void ResetPitch(Vector3 startPosition)
    {
        isInFlight = false;
        rb.useGravity = false; 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;

        if (trailRenderer != null) trailRenderer.Clear();
    }

    public void InitializePitch(PitchData data)
    {
        currentPitchType = data.Type;
        currentSpeedMPS = data.SpeedMPS;
        currentSpinRateRPM = data.SpinRateRPM;
        currentSpinAxis = data.SpinAxis;

        rb.useGravity = true; 
        rb.linearVelocity = transform.forward * currentSpeedMPS;
        isInFlight = true;
    }

    private void FixedUpdate()
    {
        if (!isInFlight) return;

        Vector3 velocity = rb.linearVelocity;
        float speed = velocity.magnitude;

        if (speed < 0.1f) 
        {
            isInFlight = false;
            return; 
        }

        // --- 1. COMMON DRAG FORCE (Applies to all pitches) ---
        float dragMagnitude = 0.5f * airDensity * (speed * speed) * dragCoefficient * ballArea;
        Vector3 dragForce = -velocity.normalized * dragMagnitude;
        rb.AddForce(dragForce, ForceMode.Force);

        // --- 2. PITCH-SPECIFIC BREAKING FORCES ---
        if (currentPitchType == PitchType.Knuckleball)
        {
            // KNUCKLEBALL LOGIC (Erratic Perlin Noise Flutter)
            // Generates a smooth but random number between -0.5 and 0.5
            float noiseX = Mathf.PerlinNoise(Time.time * flutterFrequency, 0f) - 0.5f;
            float noiseY = Mathf.PerlinNoise(0f, Time.time * flutterFrequency) - 0.5f;

            // Apply the chaotic force perpendicular to the ball's forward movement
            Vector3 flutterDirection = (transform.right * noiseX) + (transform.up * noiseY);
            Vector3 flutterForce = flutterDirection * flutterStrength;
            
            rb.AddForce(flutterForce, ForceMode.Force);
            
            // Visual Debugging for Knuckleball (Yellow line)
            Debug.DrawRay(transform.position, flutterForce.normalized * 2f, Color.yellow);
        }
        else
        {
            // STANDARD LOGIC (Magnus Spin Break for Fastballs/Breaking/Off-speed)
            float spinRadSec = currentSpinRateRPM * (Mathf.PI / 30f); 
            Vector3 spinVector = currentSpinAxis * spinRadSec;

            Vector3 magnusDirection = Vector3.Cross(spinVector, velocity).normalized;
            float magnusMagnitude = spinRadSec * speed * airDensity * ballArea * magnusScale;
            Vector3 magnusForce = magnusDirection * magnusMagnitude;

            rb.AddForce(magnusForce, ForceMode.Force);
            
            // Visual Debugging for Standard Pitches (Green line)
            Debug.DrawRay(transform.position, magnusForce.normalized * 2f, Color.green);
        }
    }
}