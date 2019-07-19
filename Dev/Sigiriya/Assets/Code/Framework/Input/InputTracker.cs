using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Andrew Rimpici
public class InputTracker : ManagerBase<InputTracker>
{
    //Gesture State Machine
    public GestureStateMachine InputStateMachine { get; private set; } = new GestureStateMachine();

    //Gesture States
    private IdleGestureState idleState = new IdleGestureState();
    private PressGestureState pressState = new PressGestureState();
    private MultiPressGestureState multipressState = new MultiPressGestureState();
    private DragGestureState dragState = new DragGestureState();

    //Gesture Transitions
    private GestureStateTransition toIdleState = new GestureStateTransition(GestureTransitionType.IDLE_TRANSITION, GestureStateType.IDLE_STATE);
    private GestureStateTransition toPressState = new GestureStateTransition(GestureTransitionType.PRESS_TRANSITION, GestureStateType.PRESS_STATE);
    private GestureStateTransition toMultipressState = new GestureStateTransition(GestureTransitionType.MULTIPRESS_TRANSITION, GestureStateType.MULTIPRESS_STATE);
    private GestureStateTransition toDragState = new GestureStateTransition(GestureTransitionType.DRAG_TRANSITION, GestureStateType.DRAG_STATE);

    private int activeTouchCount = 0;
    public List<Touch> HeldTouches { get; private set; } = new List<Touch>();

    public List<Touch> NewTouches { get; private set; } = new List<Touch>();

    private void Start()
    {
        InitStateMachine();
    }

    private void InitStateMachine()
    {
        LinkTransitions();
        AddStates();
        InputStateMachine.SetInitialStateID((int)GestureStateType.IDLE_STATE);
    }

    private void LinkTransitions()
    {
        //Idle -> Press
        idleState.AddTransition(toPressState);

        //Press -> Idle
        pressState.AddTransition(toIdleState);

        //Press -> Multipress
        pressState.AddTransition(toMultipressState);

        //Multipress -> Press
        multipressState.AddTransition(toPressState);

        //Multipress -> Idle
        multipressState.AddTransition(toIdleState);
    }

    private void AddStates()
    {
        InputStateMachine.AddState(idleState);
        InputStateMachine.AddState(pressState);
        InputStateMachine.AddState(multipressState);
        InputStateMachine.AddState(dragState);
    }

    private void Update()
    {
        InputStateMachine.Update();
        TrackTouches();
    }

    private void TrackTouches()
    {
        //If we previously had touches and now don't, all touch ended
        if (activeTouchCount != Input.touchCount && Input.touchCount == 0)
        {
            EventAnnouncer.OnTouchEnded?.Invoke();
        }
        //Iterate through the active touches and figure out their states
        else
        {
            foreach (Touch t in Input.touches)
            {
                //Debug.Log("PHASE: " + t.phase);
                //If the touch is new, fire the new touch event
                if (t.phase == TouchPhase.Began)
                {
                    EventAnnouncer.OnTouchStarted?.Invoke(t);
                    NewTouches.Add(t);
                }
                //If the touch just ended or was interupted, fire the touch release event
                else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                {
                    EventAnnouncer.OnTouchReleased?.Invoke(t);
                }
                //If the touch isn't starting or ending this frame, add touch to list
                else
                {
                    HeldTouches.Add(t);
                }
            }

            //If there are touches in the held list, fire the held touches event
            if (HeldTouches.Count > 0)
            {
                EventAnnouncer.OnTouchesHeld?.Invoke(HeldTouches.ToArray());
            }

            //Reset the touches lists to prepare for the next frame
            HeldTouches.Clear();
            NewTouches.Clear();
        }

        //To keep track of the current frames touch count during the next frame
        activeTouchCount = Input.touchCount;
    }
}
