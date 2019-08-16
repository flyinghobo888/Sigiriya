//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class MultiPressGestureState : GestureState
//{
//    public MultiPressGestureState() :
//        base(GestureStateType.MULTIPRESS_STATE)
//    { }

//    public override void OnEntrance()
//    {
//        base.OnEntrance();
//        Debug.Log("Entering Multipress Gesture State");

//        EventAnnouncer.OnMultiTouchStarted?.Invoke(InputTracker.Instance.NewTouches.ToArray());
//    }

//    public override void OnExit()
//    {
//        base.OnExit();
//        Debug.Log("Exiting Multipress Gesture State");

//        EventAnnouncer.OnMultiTouchChanging?.Invoke();
//    }

//    public override StateTransition OnUpdate()
//    {
//        StateTransition state = base.OnUpdate();

//        if (IsRunning && state == null)
//        {
//            //Multipress updated
//            EventAnnouncer.OnMultiTouchUpdated?.Invoke(InputTracker.Instance.HeldTouches.ToArray());
//            //Debug.Log("Multipress held");
//        }

//        return state;
//    }

//    protected override void ListenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted += TrackPress;
//        EventAnnouncer.OnTouchesHeld += TrackPress;
//        EventAnnouncer.OnTouchReleased += TrackPressReleased;
//        EventAnnouncer.OnTouchEnded += TransitionToIdle;
//    }

//    protected override void UnlistenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted -= TrackPress;
//        EventAnnouncer.OnTouchesHeld -= TrackPress;
//        EventAnnouncer.OnTouchReleased -= TrackPressReleased;
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

//    private void TrackPressReleased(Touch t)
//    {
//        if (InputTracker.Instance.HeldTouches.Count > 1)
//        {
//            TransitionToDeadMultiPress();
//        }
//        else
//        {
//            TrackPress();
//        }
//    }

//    private void TrackPress()
//    {
//        if (InputTracker.Instance.HeldTouches.Count < 2)
//        {
//            TransitionToDeadPress();
//        }
//        else if (Input.touchCount == 0)
//        {
//            TransitionToIdle();
//        }
//    }

//    private void TransitionToIdle()
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.IDLE_TRANSITION;
//    }

//    private void TransitionToDeadPress()
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.DEAD_PRESS_TRANSITION;
//    }

//    private void TransitionToDeadMultiPress()
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.DEAD_MULTIPRESS_TRANSITION;
//    }
//}
