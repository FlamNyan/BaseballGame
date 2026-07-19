using UnityEngine;

namespace BaseballGame.Scripts.Managers
{

    public struct PitchData
    {
        public float SpeedMPS;      // Meters per second (ready for Unity physics) 
        public float SpinRateRPM;   // Rotations per minute  
        public Vector3 SpinAxis;    // The normalized directional axis of the spin

    }

    public enum PitchSpeed { LowVelocity = 0, MediumVelocity = 1, HighVelocity = 2 }
    public enum SpinType { BackSpin = 0, TopSpin = 1, SideSpin = 2 }
    public enum SpinRate { LowSpinRate = 0, MediumSpinRate = 1, HighSpinRate = 2 }

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

        // OnApplicationQuit is called when the application is about to quit
        private void OnApplicationQuit()
        {
            _isQuitting = true;
        }

        public PitchData GenerateRandomPitch()
        {
            PitchData pitch = new PitchData();

            // 1. Calculate Speed in m/s
            float speedMph = RandomizePitchSpeed();
            pitch.SpeedMPS = speedMph * _MphToMps;

            // 2. Calculate Spin Rate
            pitch.SpinRateRPM = RandomizeSpinRate();

            // 3. Determine Spin Axis for Magnus Cross Product
            pitch.SpinAxis = GetSpinAxis(RandomizeSpinType());

            return pitch;
        }

        private float RandomizePitchSpeed()
        {
            PitchSpeed ballVelocity = (PitchSpeed)Random.Range(0, 3);
            return ballVelocity switch
            {
                PitchSpeed.LowVelocity => Random.Range(lowSpeedMin, lowSpeedMax),
                PitchSpeed.MediumVelocity => Random.Range(medSpeedMin, medSpeedMax),
                PitchSpeed.HighVelocity => Random.Range(highSpeedMin, highSpeedMax),
                _ => Random.Range(medSpeedMin, medSpeedMax)
            };
        }

        private float RandomizeSpinRate()
        {
            SpinRate spinRateTier = (SpinRate)Random.Range(0, 3);
            return spinRateTier switch
            {
                SpinRate.LowSpinRate => Random.Range(lowSpinMin, lowSpinMax),
                SpinRate.MediumSpinRate => Random.Range(medSpinMin, medSpinMax),
                SpinRate.HighSpinRate => Random.Range(highSpinMin, highSpinMax),
                _ => Random.Range(medSpinMin, medSpinMax)
            };
        }

        private SpinType RandomizeSpinType()
        {
            return (SpinType)Random.Range(0, 3);
        }

        // 2. Update this method to actually use the spinType passed into it
        private Vector3 GetSpinAxis(SpinType spinType)
        {
            // Assuming the pitcher is throwing down the global Z axis towards the batter:
            return spinType switch
            {
                SpinType.BackSpin => Vector3.right,    // Creates upward lift (Fastball)
                SpinType.TopSpin => Vector3.left,      // Creates downward dive (Curveball)
                SpinType.SideSpin => Vector3.up,       // Creates horizontal sweep (Slider/Sweeper)
                _ => Vector3.right
            };
        }

    }
}
