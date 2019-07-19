using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPressGestureState : GestureState
{
    public MultiPressGestureState() :
        base(GestureStateType.MULTIPRESS_STATE)
    { }

    public override void OnEntrance()
    {
        base.OnEntrance();
        Debug.Log("Entering Multipress Gesture State");

        EventAnnouncer.OnMultiTouchStarted?.Invoke(InputTracker.Instance.NewTouches.ToArray());
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("Exiting Multipress Gesture State");

        EventAnnouncer.OnMultiTouchChanging?.Invoke();
    }

    public override StateTransition OnUpdate()
    {
        StateTransition state = base.OnUpdate();

        if (IsRunning && state == null)
        {
            //Multipress updated
            EventAnnouncer.OnMultiTouchUpdated?.Invoke(InputTracker.Instance.HeldTouches.ToArray());
            Debug.Log("Multipress held");
        }

        return state;
    }

    protected override void ListenForEvents()
    {
        EventAnnouncer.OnTouchStarted += TrackTouch;
        EventAnnouncer.OnTouchReleased += TrackTouch;
        EventAnnouncer.OnTouchesHeld += TrackTouch;
        EventAnnouncer.OnTouchEnded += TransitionToIdle;
    }

    protected override void UnlistenForEvents()
    {
        EventAnnouncer.OnTouchStarted -= TrackTouch;
        EventAnnouncer.OnTouchReleased -= TrackTouch;
        EventAnnouncer.OnTouchesHeld -= TrackTouch;
        EventAnnouncer.OnTouchEnded -= TransitionToIdle;
    }

    private void TrackTouch(Touch[] t)
    {
        TrackTouch();
    }

    private void TrackTouch(Touch t)
    {
        TrackTouch();
    }

    private void TrackTouch()
    {
        Debug.Log("COUNT: " + Input.touchCount);
        if (Input.touchCount < 2)
        {
            TransitionToPress();
        }
        else if (Input.touchCount == 0)
        {
            TransitionToIdle();
        }
    }

    private void TransitionToIdle()
    {
        IsRunning = false;
        TransitionToUse = (int)GestureTransitionType.IDLE_TRANSITION;
    }

    private void TransitionToPress()
    {
        IsRunning = false;
        TransitionToUse = (int)GestureTransitionType.PRESS_TRANSITION;
    }
}
