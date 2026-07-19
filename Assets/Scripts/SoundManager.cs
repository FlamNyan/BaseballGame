using UnityEngine;

namespace BaseballGame.Scripts.Managers
{
    public enum PerformanceResult { Good, Ok, Bad }

    [DefaultExecutionOrder(-100)]
    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _instance;
        private static bool _isQuitting;

        public static SoundManager Instance
        {
            get
            {
                if (_isQuitting) return null;
                if (_instance == null)
                {
                    _instance = FindAnyObjectByType<SoundManager>();
                    if (_instance == null)
                    {
                        GameObject managerObject = new GameObject(nameof(SoundManager));
                        _instance = managerObject.AddComponent<SoundManager>();
                    }
                }
                return _instance;
            }
        }

        [Header("Audio Sources")]
        [Tooltip("Dedicated channel for looping music and ambience.")]
        [SerializeField] private AudioSource musicSource;
        [Tooltip("Dedicated channel for quick, overlapping sound effects.")]
        [SerializeField] private AudioSource sfxSource;

        [Header("Music & Ambience")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip crowdCheerStart;
        
        [Header("Sound Effects")]
        [Tooltip("Add multiple hit sounds here to randomize the bat crack.")]
        [SerializeField] private AudioClip[] batHitSounds;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true; 
        }

        private void Start()
        {
            PlayMenuMusic();
        }

        private void OnApplicationQuit() => _isQuitting = true;

        public void PlayMenuMusic()
        {
            if (menuMusic == null) return;
            if (musicSource.clip == menuMusic && musicSource.isPlaying) return;
            
            musicSource.clip = menuMusic;
            musicSource.Play();
        }

        public void PlayGameStartCheer()
        {
            if (crowdCheerStart == null) return;
            musicSource.clip = crowdCheerStart;
            musicSource.Play();
        }

        public void PlayBatHit()
        {
            if (batHitSounds == null || batHitSounds.Length == 0) return;

            int randomIndex = Random.Range(0, batHitSounds.Length);
            AudioClip hitSound = batHitSounds[randomIndex];

            sfxSource.PlayOneShot(hitSound);
        }
    }
}