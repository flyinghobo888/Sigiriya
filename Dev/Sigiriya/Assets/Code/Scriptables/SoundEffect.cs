using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
//Can be used to create new Sound Effects by designers/sound designers.
[CreateAssetMenu(fileName = "New Sound Effect", menuName = "Sound Effect")]
public class SoundEffect : ScriptableObject
{
    public List<AudioClip> sounds = new List<AudioClip>();
    public EnumSound soundID;

    public AudioClip GetRandomSound()
    {
        return sounds[Random.Range(0, sounds.Count)];
    }
}

public enum EnumSound
{
    BUTTON_PRESS
}
