//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class IdleGestureState : GestureState
//{
//    public IdleGestureState() :
//        base(GestureStateType.IDLE_STATE) {}

//    public override void OnEntrance()
//    {
//        base.OnEntrance();
//        Debug.Log("Entering Idle Gesture State");
//        EventAnnouncer.OnIdleStarted?.Invoke();
//    }

//    public override void OnExit()
//    {
//        base.OnExit();
//        Debug.Log("Exiting Idle Gesture State");
//        EventAnnouncer.OnIdleEnded?.Invoke();
//    }

//    public override StateTransition OnUpdate()
//    {
//        StateTransition state = base.OnUpdate();

//        if (IsRunning && state == null)
//        {
//            //Do nothing for idle
//        }

//        return state;
//    }

//    protected override void ListenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted += TransitionToPress;
//    }

//    protected override void UnlistenForEvents()
//    {
//        EventAnnouncer.OnTouchStarted -= TransitionToPress;
//    }

//    private void TransitionToPress(Touch t)
//    {
//        IsRunning = false;
//        TransitionToUse = (int)GestureTransitionType.PRESS_TRANSITION;
//    }
//}
