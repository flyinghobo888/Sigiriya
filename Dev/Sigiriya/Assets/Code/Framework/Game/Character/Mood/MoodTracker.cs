using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodTracker
{
    private Dictionary<EnumMood, MoodNode> moodList = new Dictionary<EnumMood, MoodNode>();
    private List<EnumMood> removalsList = new List<EnumMood>();

    //public void AddMood(EnumMood mood, float duration, bool isPaused = false)
    //{
    //    if (moodList.TryGetValue(mood, out MoodNode existingValue))
    //    {
    //        existingValue.SetDuration(duration);
    //        existingValue.Paused = isPaused;
    //        Debug.Log("Mood already active on this character. Updating values.");
    //    }
    //    else
    //    {
    //        MoodNode newMoodNode = MoodController.Instance.CreateMoodNode(mood, duration, isPaused);

    //        if (newMoodNode != null)
    //        {
    //            moodList.Add(mood, newMoodNode);
    //        }
    //        else
    //        {
    //            Debug.LogWarning("Could not add mood to mood tracker. Mood Node is null.");
    //        }
    //    }
    //}

    public void AddMood(EnumMood mood, SigiTime duration, bool isPaused = false)
    {
        if (moodList.TryGetValue(mood, out MoodNode existingValue))
        {
            existingValue.SetDuration(duration);
            existingValue.Paused = isPaused;
            //Debug.Log("Mood already active on this character. Updating values.");
        }
        else
        {
            MoodNode newMoodNode = MoodController.Instance.CreateMoodNode(mood, duration, isPaused);

            if (newMoodNode != null)
            {
                moodList.Add(mood, newMoodNode);
            }
            else
            {
                //Debug.LogWarning("Could not add mood to mood tracker. Mood Node is null.");
            }
        }
    }

    //We need to query it and remove it next frame otherwise the dictionary will get corrupt if we remove it in the middle of iterating over it.
    public void QueryRemoveMood(EnumMood mood)
    {
        if (moodList.TryGetValue(mood, out MoodNode existingValue))
        {
            removalsList.Add(mood);
            existingValue.DeactivateNode();
        }
        else
        {
            //Debug.Log("Can't remove mood. This character doesn't have mood: " + mood.ToString() + " already.");
        }
    }

    public bool PauseMood(EnumMood mood, bool isPaused)
    {
        if (moodList.TryGetValue(mood, out MoodNode existingValue))
        {
            existingValue.Paused = isPaused;
            return true;
        }
        else
        {
            //Debug.Log("Can't pause mood. This character doesn't have mood: " + mood.ToString());
            return false;
        }
    }

    public MoodNode GetMoodNode(EnumMood mood)
    {
        if (moodList.TryGetValue(mood, out MoodNode existingValue))
        {
            return existingValue;
        }
        else
        {
            //Debug.Log("Can't get mood. This character doesn't have mood: " + mood.ToString());
            return null;
        }
    }

    public void Update()
    {
        RemoveMoods();
        UpdateMoodNodes();
    }

    private void RemoveMoods()
    {
        foreach (EnumMood mood in removalsList)
        {
            moodList.Remove(mood);
        }

        removalsList.Clear();
    }

    private void UpdateMoodNodes()
    {
        foreach (MoodNode node in moodList.Values)
        {
            if (!node.Paused)
            {
                EnumMood mood = node.Mood;
                node.Update();

                if (!node.Active)
                {
                    QueryRemoveMood(mood);
                }
            }
        }
    }
}
