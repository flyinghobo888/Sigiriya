using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Authors: Andrew Rimpici
//Responsible for keeping track of which location in the world we're in.
public class LocationTracker : ManagerBase<LocationTracker>
{
	[SerializeField] public DialogueController dialogueController;

    [SerializeField] private EnumLocation currentLocation = EnumLocation.HOME;
    [SerializeField] private Image currentLocationBackground = null;

    public EnumLocation TargetLocation
    {
        get; private set;
    }

    private bool shouldFade;

	[Header("TEMP")]
	[SerializeField] GameObject Home = null;
	[SerializeField] GameObject Clearing = null;
	[SerializeField] GameObject Kitchen = null;
	[SerializeField] GameObject Spring = null;

	[Header ("Home Location")]
    [SerializeField] private Location home = null;

    [Header("Village Location")]
    [SerializeField] private Location village = null;

    [Header("Spring Location")]
    [SerializeField] private Location spring = null;

    [Header("Clearing Location")]
    [SerializeField] private Location clearing = null;

    [Header("Gathering Location")]
    [SerializeField] private Location gathering = null;

    [Header("Construction Location")]
    [SerializeField] private Location construction = null;

    [Header("Kitchen Location")]
    [SerializeField] private Location kitchen = null;

    [Header("Potting Location")]
    [SerializeField] private Location potting = null;

    [Header("Garden Location")]
    [SerializeField] private Location garden = null;

    [Header("Wewa Location")]
    [SerializeField] private Location wewa = null;

    private Dictionary<EnumLocation, Location> locations = new Dictionary<EnumLocation, Location>();

    [Header("Location Fade")]
    [SerializeField] private Fade locationFadeRef = null;

    private void Awake()
    {
        TargetLocation = currentLocation;

        InitLocations();
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

    private void InitLocations()
    {
        if (home)
            locations.Add(EnumLocation.HOME, home);

        if (village)
            locations.Add(EnumLocation.VILLAGE, village);

        if (spring)
            locations.Add(EnumLocation.SPRING, spring);

        if (clearing)
            locations.Add(EnumLocation.CLEARING, clearing);

        if (gathering)
            locations.Add(EnumLocation.GATHERING, gathering);

        if (construction)
            locations.Add(EnumLocation.CONSTRUCTION, construction);

        if (kitchen)
            locations.Add(EnumLocation.KITCHEN, kitchen);

        if (potting)
            locations.Add(EnumLocation.POTTING, potting);

        if (garden)
            locations.Add(EnumLocation.GARDEN, garden);

        if (wewa)
            locations.Add(EnumLocation.WEWA, wewa);
    }

    private void ChangeLocation(EnumLocation targetLocation, bool fade)
    {
        if (locations.ContainsKey(targetLocation))
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
        currentLocation = TargetLocation;
		Home.SetActive(false);
		Kitchen.SetActive(false);
		Clearing.SetActive(false);
		Spring.SetActive(false);

		if (shouldFade)
        {
            locationFadeRef.FadeOut("location_fade");
        }
        else
        {
            locationFadeRef.FadeOutNow();
        }

		Debug.Log(currentLocation);
		if (currentLocation == EnumLocation.HOME)
		{
			Home.SetActive(true);
		}
		else if (currentLocation == EnumLocation.KITCHEN)
		{
			Kitchen.SetActive(true);
		}
		else if (currentLocation == EnumLocation.GATHERING)
		{
			Clearing.SetActive(true);
		}
		else if (currentLocation == EnumLocation.SPRING)
		{
			Spring.SetActive(true);
		}

		UpdateBackground(currentLocation);
    }

    private void UpdateBackground(EnumLocation location)
    {
        Location targetLocation;
        if (locations.TryGetValue (location, out targetLocation))
        {
            //TODO: Get list of flags from a global flag thing
            //For now just use first image in location obj
            currentLocationBackground.sprite = targetLocation.values [0];
        }
    }

    public bool IsLocationRegistered(EnumLocation location)
    {
        if (locations.ContainsKey(location))
        {
            return true;
        }

        return false;
    }
}

public enum EnumLocation : int
{
    HOME,
    VILLAGE,
    SPRING,
    CLEARING,
    GATHERING,
    CONSTRUCTION,
    KITCHEN,
    POTTING,
    GARDEN,
    WEWA,
    SIZE
}

//Modifiers that might change how the location looks.
public enum EnumLocationModifier : int
{
    MORNING,
    EVENING,
    NIGHT,
    //FIRE,
    //etc
}