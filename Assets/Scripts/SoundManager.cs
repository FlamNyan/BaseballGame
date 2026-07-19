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
        [SerializeField] private AudioClip goodPerformanceMusic;
        [SerializeField] private AudioClip okPerformanceMusic;
        [SerializeField] private AudioClip badPerformanceMusic;
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

            // Safety check: Auto-create AudioSources if they weren't assigned in the Inspector
            if (musicSource == null) musicSource = gameObject.AddComponent<AudioSource>();
            if (sfxSource == null) sfxSource = gameObject.AddComponent<AudioSource>();

            // Ensure the music channel loops continuously
            musicSource.loop = true; 
        }

        private void Start()
        {
            // Start playing the menu music as soon as the manager initializes
            PlayMenuMusic();
        }

        private void OnApplicationQuit() => _isQuitting = true;

        // ==========================================
        // --- BACKGROUND MUSIC (BGM) METHODS ---
        // ==========================================

        public void PlayMenuMusic()
        {
            if (menuMusic == null) return;
            musicSource.clip = menuMusic;
            musicSource.Play();
        }

        public void PlayGameStartCheer()
        {
            if (crowdCheerStart == null) return;
            musicSource.clip = crowdCheerStart;
            musicSource.Play();
        }

        public void PlayEndGameMusic(PerformanceResult result)
        {
            AudioClip clipToPlay = result switch
            {
                PerformanceResult.Good => goodPerformanceMusic,
                PerformanceResult.Ok => okPerformanceMusic,
                PerformanceResult.Bad => badPerformanceMusic,
                _ => null
            };

            if (clipToPlay != null)
            {
                musicSource.clip = clipToPlay;
                musicSource.Play();
            }
        }

        // ==========================================
        // --- SOUND EFFECTS (SFX) METHODS ---
        // ==========================================


        public void PlayBatHit()
        {
            if (batHitSounds == null || batHitSounds.Length == 0) return;

            // Pick a random crack sound from the array
            int randomIndex = Random.Range(0, batHitSounds.Length);
            AudioClip hitSound = batHitSounds[randomIndex];

            sfxSource.PlayOneShot(hitSound);
        }
    }
}