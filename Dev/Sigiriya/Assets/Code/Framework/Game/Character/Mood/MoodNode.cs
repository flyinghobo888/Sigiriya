using System.Collections;
using System.Collections.Generic;

public class MoodNode
{
    //For MoodNodePool
    public bool Active { get; set; }

    //If it's paused, the duration won't go down, but it will still stay in the mood list for the affected character.
    public bool Paused { get; set; }

    //When duration hits 0, the mood gets removed from the character.
    public float Duration { get; private set; }
    
    //The mood for this character.
    public EnumMood Mood { get; private set; }

    //MEANT TO BE CALLED BY THE MOODNODEPOOL
    public MoodNode()
    {
        DeactivateNode();
    }

    public void InitMoodNode(EnumMood mood, float duration, bool isPaused)
    {
        Mood = mood;
        Duration = duration;
        Paused = isPaused;
    }

    public void Update(float deltaTime)
    {
        if (Duration > 0)
        {
            Duration -= deltaTime;
        }
        else
        {
            DeactivateNode();
        }
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }

    public void DeactivateNode()
    {
        Active = false;
        Paused = true;
        Duration = 0;
        Mood = EnumMood.SIZE;
    }
}

public enum EnumMood
{
	NONE = -1,
    HAPPY,
    SAD,
    EXCITED,
    ANGRY,

    SIZE
}