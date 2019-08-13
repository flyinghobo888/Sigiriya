using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class LocationController : MonoBehaviour
{
    [SerializeField] private Location locationData;
    public Location LocationData { get => locationData; private set { locationData = value; } }

    [SerializeField] private RectTransform characterContainer = null;

    //ParallaxController BackgroundParallax { get; private set; }

    //Get the random images based on flags and do parallax stuff in here when we get there.

    public bool HasEndOfDayHappened { get; private set; } = false;

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

        if (prevLocation == EnumLocation.HOME && HasEndOfDayHappened)
        {
            //Hide the endOfDay button
            LocationTracker.Instance.ShowEndOfDayButton(false);
        }
    }

    private void ArrivedAtLocation(EnumLocation currentLocation)
    {
        if (currentLocation == EnumLocation.HOME && currentLocation == locationData.locationType
            && HasEndOfDayHappened)
        {
            //Show endOfDay button
            LocationTracker.Instance.ShowEndOfDayButton(true);

            //For now restart the day
            //GlobalTimeTracker.Instance.StartDay(true);
        }
    }

    private void EnableCharacters(bool shouldEnable)
    {
        characterContainer.gameObject.SetActive(shouldEnable);
    }
}
