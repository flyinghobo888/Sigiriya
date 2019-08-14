//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PressGestureState : GestureState
//{
//    public PressGestureState() :
//        base(GestureStateType.PRESS_STATE)
//    { }

//    public override void OnEntrance()
//    {
//        base.OnEntrance();
//        Debug.Log("Entering Press Gesture State");

//        EventAnnouncer.OnPressStarted?.Invoke(InputTracker.Instance.NewTouches[0]);
//    }

//    public override void OnExit()
//    {
//        base.OnExit();
//        Debug.Log("Exiting Press Gesture State");

//        EventAnnouncer.OnPressChanging?.Invoke();
//    }

//    public override StateTransition OnUpdate()
//    {
//        StateTransition state = base.OnUpdate();

//        //The press is being held
//        if (IsRunning && state == null)
//        {
//            //We know HeldTouches size is greater than 0, otherwise the state would not be null
//            EventAnnouncer.OnPressHeld?.Invoke(InputTracker.Instance.HeldTouches[0]);
//            //Debug.Log("Press held");
//        }

//        return state;
//    }

//    protected override void ListenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted += TrackPress;
//        EventAnnouncer.OnTouchesHeld += TrackPress;
//        EventAnnouncer.OnTouchReleased += TrackPress;
//        EventAnnouncer.OnTouchEnded += TransitionToIdle;
//    }

//    protected override void UnlistenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted -= TrackPress;
//        EventAnnouncer.OnTouchesHeld -= TrackPress;
//        EventAnnouncer.OnTouchReleased -= TrackPress;
//        EventAnnouncer.OnTouchEnded -= TransitionToIdle;
//    }

//    private void TrackPress(TouchData[] t)
//    {
//        TrackPress();
//    }

//    private void TrackPress(Touch t)
//    {
//        TrackPress();
//    }

//    private void TrackPress()
//    {
//        if (Input.touchCount > 1)
//        {
//            TransitionToMultiPress();
//        }
//        else if (Input.touchCount == 0)
//        {
//            TransitionToIdle();
//        }
//    }

//    private void TransitionToMultiPress()
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.MULTIPRESS_TRANSITION;
//    }

//    private void TransitionToIdle()
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.IDLE_TRANSITION;
//    }
//}
