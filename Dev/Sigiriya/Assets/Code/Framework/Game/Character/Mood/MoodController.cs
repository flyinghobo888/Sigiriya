using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodController : ManagerBase<MoodController>
{
    private Dictionary<Character, MoodTracker> moodList = new Dictionary<Character, MoodTracker>();

    public void RegisterMoodTracker(Character character, MoodTracker tracker)
    {
        moodList.Add(character, tracker);
    }

    private void Update()
    {
        foreach (MoodTracker moodTracker in moodList.Values)
        {
            moodTracker.Update();
        }
    }

    public int nodePoolSize;
    private MoodNode[] nodePool;

    public void Start()
    {
        nodePool = new MoodNode[nodePoolSize];

        int i;
        for (i = 0; i < nodePoolSize; ++i)
        {
            nodePool[i] = new MoodNode();
            Debug.Log("INITIALIZING NODE: " + i);
        }
    }

    public MoodNode CreateMoodNode(EnumMood mood, float duration, bool isPaused = false)
    {
        MoodNode node = ActivateMoodNode();

        if (node != null)
        {
            node.InitMoodNode(mood, duration, isPaused);
            return node;
        }

        return null;
    }

    private MoodNode ActivateMoodNode()
    {
        foreach (MoodNode node in nodePool)
        {
            if (!node.Active)
            {
                node.Active = true;
                return node;
            }
        }

        Debug.LogWarning("Using the max amount of mood nodes! Either use less, or expand the MoodNodePool size.");
        return null;
    }
}
