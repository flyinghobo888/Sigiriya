using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationRegisterer : MonoBehaviour
{
    private LocationController[] locationControllers;

    private void Awake()
    {
        locationControllers = transform.GetComponentsInChildren<LocationController>(true);
        RegisterLocations();
    }

    private void RegisterLocations()
    {
        foreach (LocationController location in locationControllers)
        {
            LocationTracker.Instance.RegisterLocation(location.LocationData.locationType, location);
            location.gameObject.SetActive(false);
        }
    }
}
