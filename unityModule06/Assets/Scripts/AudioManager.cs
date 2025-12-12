using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource ambientSource;
    public AudioSource sfxSource;

    public AudioClip faintClip;
    public AudioClip winClip;
    public AudioClip[] footstepClips;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (ambientSource != null && !ambientSource.isPlaying)
            ambientSource.Play();
    }

    public void PlayFaint()
    {
        if (sfxSource && faintClip)
            sfxSource.PlayOneShot(faintClip);
    }

    public void PlayWin()
    {
        if (sfxSource && winClip)
            sfxSource.PlayOneShot(winClip);
    }

    public void PlayFootstep()
    {
        if (sfxSource == null || footstepClips.Length == 0) return;
        var clip = footstepClips[Random.Range(0, footstepClips.Length)];
        sfxSource.PlayOneShot(clip);
    }
}
