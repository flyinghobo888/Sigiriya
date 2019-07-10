using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour
{
    [SerializeField] private GameObject map;

    private void OnEnable()
    {
        EventAnnouncer.OnEndFadeIn += CloseMap;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnEndFadeIn -= CloseMap;
    }

    public void GoToKitchen(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.KITCHEN, shouldFade);
    }

    public void GoToConstructionSite(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.CONSTRUCTION, shouldFade);
    }

    public void GoToGarden(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.GARDEN, shouldFade);
    }

    public void GoToGatheringSpace(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.GATHERING, shouldFade);
    }

    public void GoToHome(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.HOME, shouldFade);
    }

    public void GoToSpring(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.SPRING, shouldFade);
    }

    public void ToggleMap()
    {
        map.SetActive(!map.activeSelf);
    }

    private void CloseMap(string fadeID)
    {
        if (fadeID.CompareTo("location_fade") == 0)
        {
            map.SetActive(false);
        }
    }
}
