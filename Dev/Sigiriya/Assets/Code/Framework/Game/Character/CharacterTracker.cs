using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTracker : ManagerBase<CharacterTracker>
{
    public List<Character> rawCharacters = new List<Character>();

    private HashSet<Character> characters = new HashSet<Character>();
    private const int MAX_LOC_SWITCH_RETRIES = 5;

    private void Awake()
    {
        characters.Clear();
        foreach (Character character in rawCharacters)
        {
            if (!characters.Add(character))
            {
                Debug.Log("Character: " + character.CharacterName + " is already registered.");
            }
            else
            {
                character.InitCharacter();
            }
        }

        Debug.Log("Registered Characters");
        foreach (Character character in characters)
        {
            Debug.Log("Character: " + character.CharacterName);
        }
    }

    private void Start()
    {
        UpdateCharacters();
    }

    private void OnEnable()
    {
        //EventAnnouncer.OnDayIsStarting += UpdateCharacters;
        EventAnnouncer.OnTimeOfDayChanged += TryUpdateCharacters;
        EventAnnouncer.OnDialogueEnd += TryUpdateCharacters;
        EventAnnouncer.OnRequestLocationChange += TryUpdateCharacters;
    }

    private void OnDisable()
    {
        //EventAnnouncer.OnDayIsStarting -= UpdateCharacters;
        EventAnnouncer.OnTimeOfDayChanged -= TryUpdateCharacters;
        EventAnnouncer.OnDialogueEnd -= TryUpdateCharacters;
        EventAnnouncer.OnRequestLocationChange -= TryUpdateCharacters;
    }

    private bool RequestUpdate = false;
    private bool HasUpdatedAlready = false;

    private void TryUpdateCharacters(EnumDisplayTime displayTime)
    {
        RequestUpdate = true;
        HasUpdatedAlready = false;
    }

    private void TryUpdateCharacters(EnumLocation prev, EnumLocation next, bool shouldFade)
    {
        if (RequestUpdate && !HasUpdatedAlready)
        {
            UpdateCharacters();
            RequestUpdate = false;
            HasUpdatedAlready = true;
        }
    }

    private void TryUpdateCharacters()
    {
        if (RequestUpdate && !HasUpdatedAlready)
        {
            UpdateCharacters();
            RequestUpdate = false;
            HasUpdatedAlready = true;
        }
    }

    private void UpdateCharacters()
    {
        //LocationTracker.Instance.CharacterScheduleUpdate();
        EventAnnouncer.OnRequestCharacterScheduleUpdate?.Invoke();

        EnumTime currentTime = GlobalTimeTracker.Instance.CurrentTimeOfDay;

        foreach (Character character in characters)
        {
            int tryCount = 0;

            Schedule schedule = character.GetSchedule(currentTime);
            EnumLocation location = schedule.GetRandomLocation();
            LocationController controller = LocationTracker.Instance.GetLocationController(location);

            //The character isn't in any location.
            if (!controller)
            {
                continue;
            }

            //Try to set a location and if it can't because all the slots are full in the desired locations, set to no location.
            while (tryCount < MAX_LOC_SWITCH_RETRIES && controller && !controller.SetRandomCharacterSlot(character, true))
            {
                ++tryCount;
            }
        }
    }
}
