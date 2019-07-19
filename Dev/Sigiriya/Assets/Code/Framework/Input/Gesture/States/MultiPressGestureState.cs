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
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override StateTransition OnUpdate()
    {
        return null;
    }

    protected override void ListenForEvents()
    {
    }

    protected override void UnlistenForEvents()
    {
    }
}
