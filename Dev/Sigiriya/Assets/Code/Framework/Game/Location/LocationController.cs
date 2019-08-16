using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class LocationController : MonoBehaviour
{
    [SerializeField] private Location locationData;
    public Location LocationData { get => locationData; private set { locationData = value; } }
    [Space]
    [SerializeField] private List<WorldCharacterHandler> characterSlotsInLocation;
    public List<WorldCharacterHandler> CharacterSlotsInLocation { get => characterSlotsInLocation; private set { characterSlotsInLocation = value; } }
    private List<WorldCharacterHandler> openCharacterSlots = new List<WorldCharacterHandler>();
    [Space]
    //public RectTransform WorldContainer = null;
    public RectTransform BG = null;
    public RectTransform MG = null;
    public RectTransform FG = null;

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
        //EnableCharacters(true);
    }

    private void EndOfDay()
    {
        HasEndOfDayHappened = true;
        DisableCharacters();
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
            //Show endOfDay button
            LocationTracker.Instance.ShowEndOfDayButton(true);

            //For now restart the day
            //GlobalTimeTracker.Instance.StartDay(true);
        }
    }

    //Figure something out now that we use a world container.
    //Maybe move all the items to another container while we disable the characters.
    private void DisableCharacters()
    {
        DisableCharacters(BG);
        DisableCharacters(MG);
        DisableCharacters(FG);
    }

    private void DisableCharacters(RectTransform layer)
    {
        Transform[] characters = layer.GetComponentsInChildren<Transform>(true);

        foreach (Transform child in characters)
        {
            if (child.gameObject.tag == "Character")
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public bool SetRandomCharacterSlot(Character character, bool resetCharactersInConvo)
    {
        if (IsCharacterSlotAvailable())
        {
            int rand = Random.Range(0, openCharacterSlots.Count);
            WorldCharacterHandler slot = openCharacterSlots[rand];
            openCharacterSlots.RemoveAt(rand);

            slot.SetCharacter(character, resetCharactersInConvo);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsCharacterSlotAvailable()
    {
        return openCharacterSlots.Count > 0;
    }

    public void ResetCharacterSlots()
    {
        foreach (WorldCharacterHandler characterSlot in CharacterSlotsInLocation)
        {
            characterSlot.SetCharacter(null, true);
        }

        openCharacterSlots.Clear();

        for (int i = 0; i < CharacterSlotsInLocation.Count; ++i)
        {
            openCharacterSlots.Add(CharacterSlotsInLocation[i]);
        }
    }
}
