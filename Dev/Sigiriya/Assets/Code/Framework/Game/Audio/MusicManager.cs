using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : ManagerBase<MusicManager>
{
    public enum EnumMusicState
    {
        ON,
        OFF
    }

    private AudioSource audioSource;

    public EnumMusicState CurrentMusicState { get; private set; } = EnumMusicState.OFF;

    public bool SetMusicState(EnumMusicState state)
    {
        if (CurrentMusicState != state)
        {
            CurrentMusicState = state;

            if (CurrentMusicState == EnumMusicState.ON)
            {
                EventAnnouncer.OnPlayMusicRequested?.Invoke();
            }
            else
            {
                EventAnnouncer.OnStopMusicRequested?.Invoke();
            }

            return true;
        }

        return false;
    }

    private void Awake()
    {
        if (!audioSource)
        {
            AudioSource source = new GameObject().AddComponent<AudioSource>();
            audioSource = Instantiate(source);
        }

        PlayMusic();
    }

    private void OnEnable()
    {
        EventAnnouncer.OnPlayMusicRequested += PlayMusic;
        EventAnnouncer.OnStopMusicRequested += StopMusic;
        EventAnnouncer.OnRequestSceneChange += ChangeMusic;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnPlayMusicRequested -= PlayMusic;
        EventAnnouncer.OnStopMusicRequested -= StopMusic;
        EventAnnouncer.OnRequestSceneChange -= ChangeMusic;
    }

    private void ChangeMusic(EnumScene scene, bool shouldFade)
    {
        Debug.Log("CHANGE MUSIC!");
        if (audioSource.isPlaying)
        {
            AudioClip newSong = GetMusicForScene(scene);
            StartCoroutine(TransitionBetweenSongs(newSong));
        }
    }

    private IEnumerator TransitionBetweenSongs(AudioClip newSong)
    {
        StartCoroutine(VolumeManager.FadeOut(audioSource, 1.0f));

        while (audioSource.isPlaying)
        {
            yield return null;
            CurrentMusicState = EnumMusicState.OFF;
        }

        if (newSong && audioSource.isActiveAndEnabled)
        {
            audioSource.clip = newSong;
            audioSource.Play();

            CurrentMusicState = EnumMusicState.ON;
        }
    }

    private void PlayMusic()
    {
        if (!audioSource.isPlaying && audioSource.isActiveAndEnabled)
        {
            AudioClip song = GetMusicForScene(SceneNavigator.Instance.CurrentScene);

            if (song)
            {
                audioSource.clip = song;
                audioSource.Play();

                CurrentMusicState = EnumMusicState.ON;
            }
            else
            {
                Debug.LogWarning("Song is null. Cannot play.");
                CurrentMusicState = EnumMusicState.OFF;
            }
        }
    }

    private void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            StartCoroutine(VolumeManager.FadeOut(audioSource, 0.5f));
        }

        CurrentMusicState = EnumMusicState.OFF;
    }

    private void Update()
    {
        if (CurrentMusicState == EnumMusicState.ON && !audioSource.isPlaying)
        {
            PlayMusic();
        }
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }

    //TEMP
    //TODO: Implement depending on how we want to do music.
    private AudioClip GetMusicForScene(EnumScene scene)
    {
        return null;
    }
}
