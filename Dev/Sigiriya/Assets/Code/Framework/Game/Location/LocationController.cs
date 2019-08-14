using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class LocationController : MonoBehaviour
{
    [SerializeField] private Location locationData;
    public Location LocationData { get => locationData; private set { locationData = value; } }

    //public RectTransform WorldContainer = null;
    public RectTransform BG = null;
    public RectTransform MG = null;
    public RectTransform FG = null;

    //Get the random images based on flags and do parallax stuff in here when we get there.

    private bool HasEndOfDayHappened = false;

    private void OnEnable()
    {
        //EventAnnouncer.OnDayIsStarting += StartOfDay;
        //EventAnnouncer.OnDayIsEnding += EndOfDay;
        EventAnnouncer.OnRequestLocationChange += UpdateLocationOnLeave;
        EventAnnouncer.OnArrivedAtLocation += ArrivedAtLocation;
    }

    private void OnDisable()
    {
        //EventAnnouncer.OnDayIsStarting -= StartOfDay;
        //EventAnnouncer.OnDayIsEnding -= EndOfDay;
        EventAnnouncer.OnRequestLocationChange -= UpdateLocationOnLeave;
        EventAnnouncer.OnArrivedAtLocation -= ArrivedAtLocation;
    }

    public void StartOfDay()
    {
        HasEndOfDayHappened = false;
        EnableCharacters(true);
    }

    private void EndOfDay()
    {
        HasEndOfDayHappened = true;
        EnableCharacters(false);
    }

    private void UpdateLocationOnLeave(EnumLocation prevLocation, EnumLocation targetLocation, bool shouldFade)
    {
        if (prevLocation != locationData.locationType)
        {
            return;
        }

        if (!HasEndOfDayHappened && GlobalTimeTracker.Instance.IsDayOver())
        {
            EndOfDay();
        }
    }

    private void ArrivedAtLocation(EnumLocation currentLocation)
    {
        //Update background based on current game flags
        //if (FlagBank.Instance.)
        {
            //TODO: Change EnumLocationModifier to flagbank stuff
            if (locationData.BackgroundData.TryGetValue(EnumLocationModifier.NORMAL, out Parallax p))
            {
                LocationTracker.Instance.BackgroundController.SetBackground(p);
            }
        }

        if (currentLocation == EnumLocation.HOME && currentLocation == locationData.locationType
            && HasEndOfDayHappened)
        {
            //For now restart the day
            GlobalTimeTracker.Instance.StartDay(true);
        }
    }

    //Figure something out now that we use a world container.
    //Maybe move all the items to another container while we disable the characters.
    private void EnableCharacters(bool shouldEnable)
    {
        EnableCharacters(shouldEnable, BG);
        EnableCharacters(shouldEnable, MG);
        EnableCharacters(shouldEnable, FG);
    }

    private void EnableCharacters(bool shouldEnable, RectTransform layer)
    {
        Transform[] characters = layer.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in characters)
        {
            if (child.gameObject.tag == "Character")
            {
                child.gameObject.SetActive(shouldEnable);
            }
        }
    }
}
