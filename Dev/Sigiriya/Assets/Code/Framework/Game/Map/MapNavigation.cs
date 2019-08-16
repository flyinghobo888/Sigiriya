using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour
{
    [SerializeField] private GameObject backgroundBlurContainer = null;
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
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.KITCHEN, shouldFade);
    }

    public void GoToWewa(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.WEWA_MARSH, shouldFade);
    }

    public void GoToConstructionSite(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.CONSTRUCTION_SITE, shouldFade);
    }

    public void GoToGatheringSpace(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.GATHERING_SPACE, shouldFade);
    }

    public void GoToForestClearing(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.FOREST_CLEARING, shouldFade);
    }

    public void GoToHome(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.HOME, shouldFade);
    }

    public void GoToVillageCenter(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.VILLAGE_CENTER, shouldFade);
    }

    public void GoToPottingYard(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.POTTING_YARD, shouldFade);
    }

    public void GoToSpring(bool shouldFade)
    {
        backgroundBlurContainer.SetActive(shouldFade);
        EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, EnumLocation.SPRING, shouldFade);
    }

    public void ToggleMap()
    {
        backgroundBlurContainer.SetActive(!map.activeSelf);
        map.SetActive(!map.activeSelf);
    }
    
    public void ShowMap(bool show)
    {
        backgroundBlurContainer.SetActive(show);
        map.SetActive(show);
    }

    private void CloseMap(string fadeID)
    {
        if (fadeID.CompareTo("location_fade") == 0)
        {
            map.SetActive(false);
            backgroundBlurContainer.SetActive(false);
        }
    }
}
