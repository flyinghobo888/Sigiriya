﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Designed to hold all of the possible events for the game.
public class EventAnnouncer : ManagerBase<EventAnnouncer>
{
    public void PressButton()
    {
        OnUIButtonPressed?.Invoke(EnumSound.BUTTON_PRESS);
    }

    /* Scene Events */

    //Sends an event to change to the target scene (Changes the scene to the target scene).
    public delegate void RequestSceneChange(EnumScene scene, bool shouldFade);
    public static RequestSceneChange OnRequestSceneChange;

    
    /* Location Events */

    public delegate void RequestLocationChange(EnumLocation prevLocation, EnumLocation targetLocation, bool shouldFade);
    public static RequestLocationChange OnRequestLocationChange;

    public delegate void ArrivedAtLocation(EnumLocation currentLocation);
    public static ArrivedAtLocation OnArrivedAtLocation;

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

    public delegate void DialogueRestart();
    public static DialogueRestart OnDialogueRestart;

    public delegate void DialogueUpdate(TextMeshProUGUI display, string text);
    public static DialogueUpdate OnDialogueUpdate;

    public delegate void DialogueRequestFinish(TextMeshProUGUI display);
    public static DialogueRequestFinish OnDialogueRequestFinish;


    /* Audio Events */

    public delegate void PlayMusicRequested();
    public static PlayMusicRequested OnPlayMusicRequested;

    public delegate void StopMusicRequested();
    public static StopMusicRequested OnStopMusicRequested;

    public delegate void MusicVolumeChanged(float value);
    public static MusicVolumeChanged OnMusicVolumeChanged;

    public delegate void SoundVolumeChanged(float value);
    public static SoundVolumeChanged OnSoundVolumeChanged;


    /* Sound Events */

    public delegate AudioSource UIButtonPressed(EnumSound soundID);
    public static UIButtonPressed OnUIButtonPressed;


    /* Input Events */

    //When the user touches the screen
    public delegate void TouchStarted(Touch t);
    public static TouchStarted OnTouchStarted;

    //When the user continues touching the screen
    public delegate void TouchesHeld(Touch[] t);
    public static TouchesHeld OnTouchesHeld;

    //When the user releases a touch
    public delegate void TouchReleased(Touch t);
    public static TouchReleased OnTouchReleased;

    //When there are no more touches on the screen
    public delegate void TouchEnded();
    public static TouchEnded OnTouchEnded;


    /* Gesture Events */

    //When the user has no fingers on the screen
    public delegate void IdleStarted();
    public static IdleStarted OnIdleStarted;

    //When the user starts interacting with the screen
    public delegate void IdleEnded();
    public static IdleEnded OnIdleEnded;

    //When the user first puts their finger on the screen
    public delegate void PressStarted(Touch touch);
    public static PressStarted OnPressStarted;

    //When the user keeps their finger on the screen
    public delegate void PressHeld(Touch touch);
    public static PressHeld OnPressHeld;

    //When the user removes their finger from the screen
    public delegate void PressReleased(Touch touch);
    public static PressReleased OnPressReleased;

    //When the user starts doing a gesture other than pressing
    public delegate void PressChanging();
    public static PressChanging OnPressChanging;

    //When the user stops dragging, or removes a finger but still has another finger on the screen
    public delegate void DeadPressStarted();
    public static DeadPressStarted OnDeadPressStarted;

    //When the user doesn't move the dead finger
    public delegate void DeadPressHeld();
    public static DeadPressHeld OnDeadPressHeld;

    //When the user moves or releases the dead finger
    public delegate void DeadPressChanging();
    public static DeadPressChanging OnDeadPressChanging;

    //When the user starts dragging their finger on the screen
    public delegate void DragStarted(Touch touch);
    public static DragStarted OnDragStarted;

    //When the user continues dragging their finger on the screen
    public delegate void DragUpdated(Touch touch);
    public static DragUpdated OnDragUpdated;

    //When the user picked up their finger after already dragging
    public delegate void DragReleased(Touch touch);
    public static DragReleased OnDragReleased;

    //When the user starts doing a gesture other than dragging
    public delegate void DragChanging(Touch touch);
    public static DragChanging OnDragChanging;

    //When the user starts touching with multiple fingers
    public delegate void MultiTouchStarted(Touch[] touch);
    public static MultiTouchStarted OnMultiTouchStarted;

    //When the user starts moving their fingers
    public delegate void MultiTouchUpdated(Touch[] touch);
    public static MultiTouchUpdated OnMultiTouchUpdated;

    //When the user removes all their fingers
    public delegate void MultiTouchEnded(Touch[] touch);
    public static MultiTouchEnded OnMultiTouchEnded;

    //When the user removes too many fingers and only has one left
    public delegate void MultiTouchChanging();
    public static MultiTouchChanging OnMultiTouchChanging;

    //When the user stops dragging, or removes a finger but still has other fingers on the screen
    public delegate void DeadMultiPressStarted();
    public static DeadMultiPressStarted OnDeadMultiPressStarted;

    //When the user doesn't move the dead fingers
    public delegate void DeadMultiPressHeld();
    public static DeadMultiPressHeld OnDeadMultiPressHeld;

    //When the user moves or releases the dead fingers
    public delegate void DeadMultiPressChanging();
    public static DeadMultiPressChanging OnDeadMultiPressChanging;


    /* Global Time Events */

    public delegate void TimeOfDayChanged(EnumDisplayTime currentTime);
    public static TimeOfDayChanged OnTimeOfDayChanged;

    public delegate void TimeTickUpdated(SigiTime globalTime);
    public static TimeTickUpdated OnTimeTickUpdated;

    public delegate void TimeTransitioning(EnumDisplayTime prev, EnumDisplayTime next, float alpha);
    public static TimeTransitioning OnTimeTransitioning;

    public delegate void DayIsStarting();
    public static DayIsStarting OnDayIsStarting;

    public delegate void DayIsEnding();
    public static DayIsEnding OnDayIsEnding;
}
