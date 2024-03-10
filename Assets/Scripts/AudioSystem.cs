using UnityEngine;

public class AudioSystem : MonoBehaviour
{
    static AudioSource source;
    public AudioClip scarySound;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public static void Play(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public static void Play(AudioClip clip, float soundVolume)
    {
        source.PlayOneShot(clip, soundVolume);
    }

    async void Start()
    {
        // Play scary sounds
        while (true)
        {
            await new WaitForSeconds(Random.Range(10f, 20f));
            source.PlayOneShot(scarySound, 10f);
        }
    }
}