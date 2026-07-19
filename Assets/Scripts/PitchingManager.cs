using UnityEngine;

namespace BaseballGame.Scripts.Managers
{
    public enum PitchSpeed
    {
        LowVelocity = 0,
        MediumVelocity = 1,
        HighVelocity = 2
    }

    public enum SpinType
    {
        BackSpin = 0,
        TopSpin = 1,
        SideSpin = 2
    }

    public enum SpinRate
    {
        LowSpinRate = 0,
        MediumSpinRate = 1,
        HighSpinRate = 2
    }

    // Ensures that the PitchingManager is initialized before any other scripts that depend on it
    [DefaultExecutionOrder(-100)] 
    public class PitchingManager : MonoBehaviour
    {
        // Singleton instance of the PitchingManager
        private static PitchingManager _instance;
        // Flag to indicate if the application is quitting, to prevent creating a new instance during shutdown
        private static bool _isQuitting;

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

        private void RandomizePitchSpeed()
        {
            PitchSpeed ballVelocity = (PitchSpeed)UnityEngine.Random.Range(0, 3);

            switch (ballVelocity)
            {
                case PitchSpeed.LowVelocity:

                case PitchSpeed.MediumVelocity:

                case PitchSpeed.HighVelocity:

                default:
                    break;
            }     
        }

        private void RandomizeSpinType()
        {
            SpinType spinType = (SpinType)UnityEngine.Random.Range(0, 3);

            switch (spinType)
            {
                case SpinType.BackSpin:

                case SpinType.TopSpin:

                case SpinType.SideSpin:

                default:
                    break;
            }     
        }

        private void RandomizeSpinRate()
        {
            SpinRate spinRate = (SpinRate)UnityEngine.Random.Range(0, 3);

            switch (spinRate)
            {
                case SpinRate.LowSpinRate :

                case SpinRate.MediumSpinRate:

                case SpinRate.HighSpinRate:

                default:
                    break;
            }     
        }
    }
}
