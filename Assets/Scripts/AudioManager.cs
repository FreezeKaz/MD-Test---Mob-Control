using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private int poolSize = 300;       // how many AudioSources to create
    private AudioSource[] sources;
    private int currentIndex = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        sources = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            GameObject SFX = new GameObject("SFXSource_" + i);
            SFX.transform.parent = transform;
            sources[i] = SFX.AddComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource source = sources[currentIndex];
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();

        currentIndex = (currentIndex + 1) % poolSize; // cycle through pool
    }
}