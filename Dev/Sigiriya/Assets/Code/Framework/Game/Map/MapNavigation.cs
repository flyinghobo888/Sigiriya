using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour
{
    [SerializeField] private GameObject map = null;

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

    public void GoToWewa(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.WEWA_MARSH, shouldFade);
    }

    public void GoToConstructionSite(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.CONSTRUCTION_SITE, shouldFade);
    }

    public void GoToGatheringSpace(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.GATHERING_SPACE, shouldFade);
    }

    public void GoToForestClearing(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.FOREST_CLEARING, shouldFade);
    }

    public void GoToHome(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.HOME, shouldFade);
    }

    public void GoToVillageCenter(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.VILLAGE_CENTER, shouldFade);
    }

    public void GoToPottingYard(bool shouldFade)
    {
        EventAnnouncer.OnRequestLocationChange?.Invoke(EnumLocation.POTTING_YARD, shouldFade);
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
