using UnityEngine;
using UnityEngine.InputSystem;

namespace BaseballGame.Scripts.Managers
{

    public struct PitchData
    {
        public PitchType Type;      // Tell the ball what type of pitch it is
        public float SpeedMPS;      
        public float SpinRateRPM;     
        public Vector3 SpinAxis;    
    }

    // The 5 main pitches
    public enum PitchType { FourSeamFastball, Curveball, Slider, Changeup, Knuckleball }

    // Ensures that the PitchingManager is initialized before any other scripts that depend on it
    [DefaultExecutionOrder(-100)] 
    public class PitchingManager : MonoBehaviour
    {
        // Singleton instance of the PitchingManager
        private static PitchingManager _instance;
        // Flag to indicate if the application is quitting, to prevent creating a new instance during shutdown
        private static bool _isQuitting;

        // Conversion constant: 1 MPH = 0.44704 Meters Per Second
        private const float _MphToMps = 0.44704f;

        [Header("MLB Velocity Ranges (MPH)")]
        [SerializeField] private float lowSpeedMin = 70f;
        [SerializeField] private float lowSpeedMax = 80f;
        [SerializeField] private float medSpeedMin = 81f;
        [SerializeField] private float medSpeedMax = 90f;
        [SerializeField] private float highSpeedMin = 91f;
        [SerializeField] private float highSpeedMax = 102f;

        [Header("MLB Spin Rate Ranges (RPM)")]
        [SerializeField] private float lowSpinMin = 1000f;
        [SerializeField] private float lowSpinMax = 1800f;
        [SerializeField] private float medSpinMin = 1801f;
        [SerializeField] private float medSpinMax = 2400f;
        [SerializeField] private float highSpinMin = 2401f;
        [SerializeField] private float highSpinMax = 3000f;

        // Singleton pattern to ensure only one instance of PitchingManager exists
        public static PitchingManager Instance
        {
            // If the application is quitting, return null to avoid creating a new instance
            get
            {
                if (_isQuitting)
                {
                    return null;
                }

                // If the instance is null, try to find an existing PitchingManager in the scene
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<PitchingManager>();
                    
                    // If no instance is found, create a new GameObject and attach the PitchingManager component to it
                    if (_instance == null)
                    {
                        GameObject managerObject = new GameObject(nameof(PitchingManager));
                        _instance = managerObject.AddComponent<PitchingManager>();    
                    }
                }

                return _instance;
            }
        }

        // Awake is called when the script instance is being loaded
        private void Awake()
        {
            // If an instance already exists and it's not this one, destroy this game object to enforce the singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            // Make sure a keyboard is connected, then check if space was pressed
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // Find the ball in the scene
                BaseballPhysics ball = FindAnyObjectByType<BaseballPhysics>();
                
                if (ball != null)
                {
                    // 1. Perfectly scrub and reset the ball
                    ball.ResetPitch(transform.position); 

                    // 2. Generate the payload and fire
                    PitchData newPitch = GenerateRandomPitch();
                    ball.InitializePitch(newPitch);
                    
                    Debug.Log($"Pitch Thrown! Speed: {newPitch.SpeedMPS} m/s | Spin: {newPitch.SpinRateRPM} RPM | Axis: {newPitch.SpinAxis}");
                }
            }
        }

        // OnApplicationQuit is called when the application is about to quit
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public PitchData GenerateRandomPitch()
        {
            PitchData pitch = new PitchData();
            
            // Randomly select one of the 5 pitch types
            pitch.Type = (PitchType)Random.Range(0, 5);

            // Assign the physical profile based on the chosen pitch
            switch (pitch.Type)
            {
                case PitchType.FourSeamFastball:
                    pitch.SpeedMPS = Random.Range(92f, 100f) * _MphToMps;
                    pitch.SpinRateRPM = Random.Range(2200f, 2500f);
                    pitch.SpinAxis = Vector3.left; // Pure backspin (fights gravity)
                    break;

                case PitchType.Curveball:
                    pitch.SpeedMPS = Random.Range(76f, 83f) * _MphToMps;
                    pitch.SpinRateRPM = Random.Range(2400f, 2900f);
                    pitch.SpinAxis = Vector3.right; // Pure topspin (snaps downward)
                    break;

                case PitchType.Slider:
                    pitch.SpeedMPS = Random.Range(82f, 89f) * _MphToMps;
                    pitch.SpinRateRPM = Random.Range(2300f, 2800f);
                    pitch.SpinAxis = Vector3.up; // Sidespin (sweeps horizontally)
                    break;

                case PitchType.Changeup:
                    // Looks like a fastball (backspin), but thrown much slower to mess with batter timing
                    pitch.SpeedMPS = Random.Range(80f, 86f) * _MphToMps;
                    pitch.SpinRateRPM = Random.Range(1500f, 1900f);
                    pitch.SpinAxis = Vector3.left; 
                    break;

                case PitchType.Knuckleball:
                    pitch.SpeedMPS = Random.Range(65f, 75f) * _MphToMps;
                    pitch.SpinRateRPM = Random.Range(10f, 100f); // Almost no spin
                    pitch.SpinAxis = Vector3.zero; // Neutral axis
                    break;
            }

            return pitch;
        }
    }
}
