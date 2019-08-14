using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Schedule", menuName = "Schedule")]
public class Schedule : ScriptableObject
{
    [Header("Time of day for schedule.")]
    public EnumTime timeOfDay;

    [Header("Valid Locations.")]
    public List<EnumLocation> validLocations = new List<EnumLocation>();

    public EnumLocation GetRandomLocation()
    {
        return validLocations[Random.Range(0, validLocations.Count)];
    }
}
