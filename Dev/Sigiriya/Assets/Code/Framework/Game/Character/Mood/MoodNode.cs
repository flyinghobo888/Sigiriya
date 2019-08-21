using System.Collections;
using System.Collections.Generic;

public class MoodNode
{
    //For MoodNodePool
    public bool Active { get; set; }

    //If it's paused, the duration won't go down, but it will still stay in the mood list for the affected character.
    public bool Paused { get; set; }

    //When duration hits 0, the mood gets removed from the character.
    //public float Duration { get; private set; }
    public SigiTime Duration { get; private set; }
    
    //The mood for this character.
    public EnumMood Mood { get; private set; }

    //MEANT TO BE CALLED BY THE MOODNODEPOOL
    public MoodNode()
    {
        DeactivateNode();
    }

    public void InitMoodNode(EnumMood mood, SigiTime duration, bool isPaused)
    {
        Mood = mood;
        Duration = duration;
        Paused = isPaused;
    }

    public void Update()
    {
        if (Duration.CountDown())
        {
            DeactivateNode();
        }
    }

    public void SetDuration(SigiTime duration)
    {
        Duration = duration;
    }

    public void DeactivateNode()
    {
        Active = false;
        Paused = true;
        Duration = null;
        Mood = EnumMood.SIZE;
    }
}

public enum EnumMood
{
	NONE,
    ANGRY,
    ANNOYED,
    EMBARRASED,
    EXITED,
    FRAGILE,
    FURIOUS,
    GRATEFUL,
    HURT,
    NERVOUS,
    ONEDGE,
    SAD,
    SMUG,
    SPOOKED,
    STRESSED,
    SURPRISED,
    THOUGHTFUL,
    UNSURE,
    UPSET,
    WARY,
    SIZE
}