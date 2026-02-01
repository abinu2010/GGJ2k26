using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip crossBowHit;
    public AudioClip enemyHit;
    public AudioClip footsteps;
    public AudioClip inGameMusic;
    public AudioClip magicHit;
    public AudioClip maskFound;
    public AudioClip swordHit;
    public AudioClip uiButton;
    public AudioClip menuMusic;

    private AudioSource musicSource;
    private AudioSource soundEffectSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            soundEffectSource = gameObject.AddComponent<AudioSource>();

            musicSource.loop = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(inGameMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        soundEffectSource.PlayOneShot(clip);
    }
}