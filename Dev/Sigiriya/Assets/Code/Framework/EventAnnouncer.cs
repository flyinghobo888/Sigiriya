using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Authors: Andrew Rimpici
//Designed to hold all of the possible events for the game.
public class EventAnnouncer : ManagerBase<EventAnnouncer>
{
    /* Scene Events */

    //Sends an event to change to the target scene (Changes the scene to the target scene).
    public delegate void RequestSceneChange(EnumScene scene, bool shouldFade);
    public static RequestSceneChange OnRequestSceneChange;

    
    /* Location Events */

    public delegate void RequestLocationChange(EnumLocation scene, bool shouldFade);
    public static RequestLocationChange OnRequestLocationChange;


    /* Fade Events */

    public delegate void StartFadeIn(string fadeID);
    public static StartFadeIn OnStartFadeIn;

    public delegate void EndFadeIn(string fadeID);
    public static EndFadeIn OnEndFadeIn;

    public delegate void StartFadeOut(string fadeID);
    public static StartFadeOut OnStartFadeOut;

    public delegate void EndFadeOut(string fadeID);
    public static EndFadeOut OnEndFadeOut;


    /* Dialogue Controller Events */

    public delegate void ThrowFlag(FlagBank.Flags throwFlag);
    public static ThrowFlag OnThrowFlag;

    public delegate void DialogueEnd();
    public static DialogueEnd OnDialogueEnd;


    /* Text Controller Events */

    public delegate void DialogueUpdate(TextMeshProUGUI display, string text);
    public static DialogueUpdate OnDialogueUpdate;

    public delegate void DialogueRequestFinish(TextMeshProUGUI display);
    public static DialogueRequestFinish OnDialogueRequestFinish;
}
