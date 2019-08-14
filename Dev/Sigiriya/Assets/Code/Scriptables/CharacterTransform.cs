using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTransform : ObjectTransform
{
    //if you want to make for a specific mood, change the mood and it will use that transform instead.
    //None means this transform will work for all moods
    //For now I'm not implementing this unless it's needed
    [SerializeField] public EnumMood Mood = EnumMood.NONE;

    //Uses the tags to set the transforms.
    [SerializeField] public List<ObjectTransform> Children = new List<ObjectTransform>();

    public void AddObject(ObjectTransform transform)
    {
        Children.Add(transform);
    }

    public void AddMood(EnumMood mood)
    {
        Mood = mood;
    }
}
