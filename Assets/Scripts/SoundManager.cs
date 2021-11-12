using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager Instance
    {
        get { return _instance; }
    }
    public int BankSize;
    private List<AudioSource> _soundClips;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;

        _soundClips = new List<AudioSource>();
        for (int i = 0; i < BankSize; i++)
        {
            _soundClips.Add(GenerateSoundInstance());
        }
    }

    private AudioSource GenerateSoundInstance()
    {
        GameObject soundInstance = new GameObject("sound");
        soundInstance.transform.parent = this.transform;
        return soundInstance.AddComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float volume = 1f, bool randomPitch = false)
    {
        for (int i = 0; i < _soundClips.Count; i++)
        {
            if (!_soundClips[i].isPlaying)
            {
                SetAudioClip(clip, volume, _soundClips[i], randomPitch);
                return;
            }
        }

        SetAudioClip(clip, volume, GenerateSoundInstance(), randomPitch);
    }

    private void SetAudioClip(AudioClip clip, float volume, AudioSource sc, bool randomPitch)
    {
        sc.clip = clip;
        sc.volume = Random.Range(volume - .1f, volume + .1f);
        if (randomPitch)
            sc.pitch = Random.Range(.85f, 1.25f);
        else
            sc.pitch = 1f;

        sc.Play();
    }
}
