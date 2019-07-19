using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMultipressGestureState : GestureState
{
    public DeadMultipressGestureState() :
        base(GestureStateType.DEAD_MULTIPRESS_STATE)
    { }

    public override void OnEntrance()
    {
        base.OnEntrance();
        Debug.Log("Entering Dead Multipress Gesture State");

        EventAnnouncer.OnDeadMultiPressStarted?.Invoke();
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Dead Multipress Gesture State");

        EventAnnouncer.OnDeadMultiPressChanging?.Invoke();
    }

    public override StateTransition OnUpdate()
    {
        StateTransition state = base.OnUpdate();

        //The dead multi press is being held
        if (IsRunning && state == null)
        {
            EventAnnouncer.OnDeadMultiPressHeld?.Invoke();
            //Debug.Log("Dead multi press held");
        }

        return state;
    }

    protected override void ListenForEvents()
    {
        EventAnnouncer.OnTouchStarted += TrackPressStarted;
        EventAnnouncer.OnTouchesHeld += TrackPress;
        EventAnnouncer.OnTouchReleased += TrackPressReleased;
        EventAnnouncer.OnTouchEnded += TransitionToIdle;
    }

    protected override void UnlistenForEvents()
    {
        EventAnnouncer.OnTouchStarted -= TrackPressStarted;
        EventAnnouncer.OnTouchesHeld -= TrackPress;
        EventAnnouncer.OnTouchReleased -= TrackPressReleased;
        EventAnnouncer.OnTouchEnded -= TransitionToIdle;
    }

    private void TrackPress(Touch[] t)
    {
        //Check for drag here in the future maybe
    }

    private void TrackPressReleased(Touch t)
    {
        if (InputTracker.Instance.HeldTouches.Count == 1)
        {
            TransitionToDeadPress();
        }
        else if(Input.touchCount == 0)
        {
            TransitionToIdle();
        }
    }

    private void TrackPressStarted(Touch t)
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

    private void TransitionToDeadPress()
    {
        IsRunning = false;
        TransitionToUse = (int)GestureTransitionType.DEAD_PRESS_TRANSITION;
    }
}
