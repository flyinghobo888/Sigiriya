using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GestureTest : ManagerBase<GestureTest>
{
    public TextMeshProUGUI gestureLabel;
    public TextMeshProUGUI touchCountLabel;
    public TextMeshProUGUI heldTouchCoundLabel;

    public void DoUpdate()
    {
        UpdateGestureLabel();
        UpdateTouchLabels();
    }

    private void UpdateGestureLabel()
    {
        gestureLabel.text = InputTracker.Instance.InputStateMachine.CurrentState.ToString();
    }

    private void UpdateTouchLabels()
    {
        touchCountLabel.text = Input.touchCount.ToString();
        heldTouchCoundLabel.text = InputTracker.Instance.HeldTouches.Count.ToString();
    }
}
