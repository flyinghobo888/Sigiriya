using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadPressGestureState : GestureState
{
    public DeadPressGestureState() :
        base(GestureStateType.DEAD_PRESS_STATE)
    { }

    public override void OnEntrance()
    {
        base.OnEntrance();
        Debug.Log("Entering Dead Press Gesture State");

        EventAnnouncer.OnDeadPressStarted?.Invoke();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Dead Press Gesture State");

        EventAnnouncer.OnDeadPressChanging?.Invoke();
    }

    public override StateTransition OnUpdate()
    {
        StateTransition state = base.OnUpdate();

        //The press is being held
        if (IsRunning && state == null)
        {
            EventAnnouncer.OnDeadPressHeld?.Invoke();
            //Debug.Log("Dead press held");
        }

        return state;
    }

    protected override void ListenForEvents()
    {
        EventAnnouncer.OnTouchStarted += TrackPress;
        EventAnnouncer.OnTouchReleased += TrackPress;
        EventAnnouncer.OnTouchesHeld += TrackPress;
        EventAnnouncer.OnTouchEnded += TransitionToIdle;
    }

    protected override void UnlistenForEvents()
    {
        EventAnnouncer.OnTouchStarted -= TrackPress;
        EventAnnouncer.OnTouchReleased -= TrackPress;
        EventAnnouncer.OnTouchesHeld -= TrackPress;
        EventAnnouncer.OnTouchEnded -= TransitionToIdle;
    }

    private void TrackPress(Touch[] t)
    {
        TrackPress();
    }

    private void TrackPress(Touch t)
    {
        TrackPress();
    }

    //TODO: Add drag here
    private void TrackPress()
    {
        if (InputTracker.Instance.HeldTouches.Count > 1)
        {
            TransitionToMultiPress();
        }
        else if (Input.touchCount == 0)
        {
            TransitionToIdle();
        }
    }

    private void TransitionToMultiPress()
    {
        IsRunning = false;
        TransitionToUse = (int)GestureTransitionType.MULTIPRESS_TRANSITION;
    }

    private void TransitionToIdle()
    {
        IsRunning = false;
        TransitionToUse = (int)GestureTransitionType.IDLE_TRANSITION;
    }
}
