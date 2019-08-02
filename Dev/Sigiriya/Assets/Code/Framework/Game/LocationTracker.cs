using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Responsible for keeping track of which location in the world we're in.
public class LocationTracker : ManagerBase<LocationTracker>
{
    [SerializeField] private EnumLocation currentLocation = EnumLocation.HOME;

    public EnumLocation TargetLocation { get; private set; }
    private bool shouldFade;

    private Dictionary<EnumLocation, LocationController> locationControllers = new Dictionary<EnumLocation, LocationController>();

    [Header("Location Fade")]
    [SerializeField] private Fade locationFadeRef = null;

    private void Awake()
    {
        TargetLocation = currentLocation;

        ChangeLocation(currentLocation, false);
    }

    private void OnEnable()
    {
        EventAnnouncer.OnRequestLocationChange += ChangeLocation;
        EventAnnouncer.OnEndFadeIn += GoToNextLocation;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnRequestLocationChange -= ChangeLocation;
        EventAnnouncer.OnEndFadeIn -= GoToNextLocation;
    }

    public void RegisterLocation(EnumLocation locationKey, LocationController locationValue)
    {
        if (IsLocationRegistered(locationKey))
        {
            Debug.LogWarning("A location has already been registered with key: " + locationKey.ToString());
        }
        else
        {
            Debug.Log("Registering Location: " + locationKey.ToString());
            locationControllers.Add(locationKey, locationValue);
        }
    }

    private void ChangeLocation(EnumLocation targetLocation, bool fade)
    {
        if (locationControllers.ContainsKey(targetLocation))
        {
            TargetLocation = targetLocation;
            shouldFade = fade;

            if (shouldFade)
            {
                locationFadeRef.FadeIn("location_fade");
            }
            else
            {
                GoToNextLocation();
            }
        }
        else
        {
            Debug.Log("Location: " + targetLocation + " is not registered.");
        }
    }

    private void GoToNextLocation(string fadeID)
    {
        if (fadeID.CompareTo("location_fade") == 0)
        {
            GoToNextLocation();
        }
    }

    private void GoToNextLocation()
    {
        GetLocationController(currentLocation).gameObject.SetActive(false);
        currentLocation = TargetLocation;
        GetLocationController(currentLocation).gameObject.SetActive(true);

        if (shouldFade)
        {
            locationFadeRef.FadeOut("location_fade");
        }
        else
        {
            locationFadeRef.FadeOutNow();
        }
    }

    public bool IsLocationRegistered(EnumLocation location)
    {
        if (locationControllers.ContainsKey(location))
        {
            return true;
        }

        return false;
    }

    private LocationController GetLocationController(EnumLocation location)
    {
        if (locationControllers.TryGetValue(location, out LocationController locationController))
        {
            return locationController;
        }
        else
        {
            Debug.LogWarning("Could not find location registerd with key: " + location.ToString());
            return null;
        }
    }
}

public enum EnumLocation : int
{
    KITCHEN,
    WEWA_MARSH,
    CONSTRUCTION_SITE,
    GATHERING_SPACE,
    FOREST_CLEARING,
    HOME,
    VILLAGE_CENTER,
    POTTING_YARD,
    SPRING,
    SIZE
}

//Modifiers that might change how the location looks.
//Should use the flag bank maybe. Gotta talk to Karim about that
public enum EnumLocationModifier : int
{
    MORNING,
    EVENING,
    NIGHT,
    //FIRE,
    //etc
}