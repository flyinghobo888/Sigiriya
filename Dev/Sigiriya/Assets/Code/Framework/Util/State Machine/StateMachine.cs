using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; protected set; }
    public int InitialStateID { get; protected set; }

    protected Dictionary<int, State> states = new Dictionary<int, State>();

    public StateMachine()
    {
        CurrentState = null;
        InitialStateID = -1;
    }

    public void AddState(State state)
    {
        if (states.ContainsValue(state))
        {
            Debug.LogWarning("State is already added.");
        }
        else
        {
            states.Add(state.StateID, state);
        }
    }

    public void SetInitialStateID(int initialStateID)
    {
        InitialStateID = initialStateID;
    }

    public void Update()
    {
        if (CurrentState == null)
        {
            Start();
        }

        if (CurrentState != null)
        {
            StateTransition transition = CurrentState.OnUpdate();
            if (transition != null)
            {
                TransitionToState(transition.TargetStateID);
            }
        }
    }

    private void Start()
    {
        if (InitialStateID != StateTransition.INVALID_TRANSITION_TYPE)
        {
            TransitionToState(InitialStateID);
        }
        else
        {
            Debug.LogWarning("Cannot start an invalid state.");
        }
    }

    private void TransitionToState(int targetStateID)
    {
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }

        State previousState = CurrentState;
        State currentState = null;
        if (states.TryGetValue(targetStateID, out currentState))
        {
            CurrentState = currentState;
            CurrentState.SetPreviousState(previousState);
            CurrentState.OnEntrance();
        }
        else
        {
            Debug.LogWarning("Invalid targetID: " + targetStateID + ". Could not find state.");
        }
    }
}
