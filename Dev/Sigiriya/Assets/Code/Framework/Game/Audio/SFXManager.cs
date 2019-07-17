using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : ManagerBase<SFXManager>
{
    public enum EnumSFXState
    {
        ON,
        OFF
    }

    private List<AudioSource> sfxPool = new List<AudioSource>();

    [SerializeField] private int poolSize = 10;

    private AudioSource audioSourcePrefab;

    //List of supported sounds
    //[SerializeField] private SoundEffect buttonPressSound;
    [SerializeField] private List<SoundEffect> soundEffects = new List<SoundEffect>();

    private Dictionary<EnumSound, SoundEffect> sounds = new Dictionary<EnumSound, SoundEffect>();

    public EnumSFXState CurrentSFXState { get; private set; }
    private bool isInitialized = false;

    public bool SetSoundState(EnumSFXState state)
    {
        if (CurrentSFXState != state)
        {
            if (state == EnumSFXState.ON)
            {
                StartListeningForSounds();
            }
            else
            {
                StopListeningForSounds();
            }

            return true;
        }

        return false;
    }

    private void Awake()
    {
        InitSounds();
    }

    private void InitSounds()
    {
        if (!isInitialized)
        {
            sounds.Clear();
            sfxPool.Clear();

            GameObject audioSource = new GameObject();
            audioSourcePrefab = audioSource.AddComponent<AudioSource>();

            int i = 0;
            for (; i < poolSize; ++i)
            {
                sfxPool.Add(Instantiate(audioSourcePrefab, gameObject.transform));
            }

            foreach (SoundEffect sound in soundEffects)
            {
                sounds.Add(sound.soundID, sound);
            }

            isInitialized = true;
        }
    }

    private void OnEnable()
    {
        StartListeningForSounds();
    }

    private void OnDisable()
    {
        if (CurrentSFXState == EnumSFXState.ON)
        {
            StopListeningForSounds();
            sounds.Clear();
            sfxPool.Clear();
            isInitialized = false;
        }
    }

    public void ChangeVolume(float volume)
    {
        foreach (AudioSource source in sfxPool)
        {
            source.volume = volume;
        }
    }

    private void StartListeningForSounds()
    {
        CurrentSFXState = EnumSFXState.ON;

        EventAnnouncer.OnUIButtonPressed += PlaySound;
    }

    private void StopListeningForSounds()
    {
        CurrentSFXState = EnumSFXState.OFF;

        EventAnnouncer.OnUIButtonPressed -= PlaySound;
    }

    private AudioSource PlaySound(EnumSound soundID)
    {
        SoundEffect soundEffect = null;

        if (sounds.TryGetValue(soundID, out soundEffect))
        {
            AudioSource audioSource = GetAvailableAudioSource();

            if (audioSource)
            {
                AudioClip sound = soundEffect.GetRandomSound();

                if (sound)
                {
                    audioSource.clip = sound;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning("Could not find sound effect for sound ID: " + soundID.ToString());
                }
            }

            return audioSource;
        }

        Debug.LogWarning("Unable to find sound with ID: " + soundID.ToString());
        return null;
    }

    private AudioSource GetAvailableAudioSource()
    {
        foreach (AudioSource source in sfxPool)
        {
            if (!source.isPlaying)
            {
                return source;
            }
        }

        Debug.Log("All audio sources in the sfxPool are being used right now! Returning null.");
        return null;
    }
}
