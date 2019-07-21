using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class State
{
    public int StateID { get; private set; }
    public String StateName { get; private set; }
    public bool IsRunning { get; protected set; }
    public State PreviousState { get; private set; }
    public int TransitionToUse { get; protected set; }

    protected Dictionary<int, StateTransition> transitions = new Dictionary<int, StateTransition>();

    public State(int stateID)
    {
        StateID = stateID;
        StateName = ((GestureStateType[])Enum.GetValues(typeof(GestureStateType)))[StateID].ToString();
        IsRunning = false;
        TransitionToUse = StateTransition.INVALID_TRANSITION_TYPE;
        PreviousState = null;
    }

    public void AddTransition(StateTransition transition)
    {
        transitions.Add(transition.TransitionID, transition);
    }

    public void SetPreviousState(State prevState)
    {
        PreviousState = prevState;
    }

    public virtual void OnEntrance()
    {
        IsRunning = true;
        ListenForEvents();
    }

    public virtual void OnExit()
    {
        IsRunning = false;
        UnlistenForEvents();
    }

    public virtual StateTransition OnUpdate()
    {
        if (!IsRunning)
        {
            StateTransition state;
            if (transitions.TryGetValue(TransitionToUse, out state))
            {
                return state;
            }
        }

        return null;
    }

    protected abstract void ListenForEvents();

    protected abstract void UnlistenForEvents();

    public override string ToString()
    {
        return StateName;
    }
}
