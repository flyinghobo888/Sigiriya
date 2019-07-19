using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateTransition
{
    public const int INVALID_TRANSITION_TYPE = -1;

    public int TransitionID { get; private set; }
    public int TargetStateID { get; private set; }

    public StateTransition(int transitionID, int targetStateID)
    {
        TransitionID = transitionID;
        TargetStateID = targetStateID;
    }
}
