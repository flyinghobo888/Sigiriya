using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GestureStateType : int
{
    INVALID_STATE_TYPE  = -1,

    IDLE_STATE          =  0,
    PRESS_STATE         =  1,
    DRAG_STATE          =  2,
    MULTIPRESS_STATE    =  3
}

public enum GestureTransitionType : int
{
    INVALID_TRANSITION_TYPE = -1,

    IDLE_TRANSITION         =  0,
    PRESS_TRANSITION        =  1,
    DRAG_TRANSITION         =  2,
    MULTIPRESS_TRANSITION   =  3
}

public class GestureStateTransition : StateTransition
{
    public GestureStateTransition(GestureTransitionType transitionID, GestureStateType targetStateID) :
        base((int)transitionID, (int)targetStateID) { }
}

public abstract class GestureState : State
{
    public GestureState(GestureStateType stateID) :
        base((int)stateID) { }
}

public class GestureStateMachine : StateMachine
{
    public GestureStateMachine() :
        base() { }
}
