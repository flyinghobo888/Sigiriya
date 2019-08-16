using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTracker : ManagerBase<InputTracker>
{
    ////Gesture State Machine
    //public GestureStateMachine InputStateMachine { get; private set; } = new GestureStateMachine();

    ////Gesture States
    //private IdleGestureState idleState = new IdleGestureState();
    //private PressGestureState pressState = new PressGestureState();
    //private MultiPressGestureState multipressState = new MultiPressGestureState();
    //private DragGestureState dragState = new DragGestureState();
    //private DeadPressGestureState deadPressState = new DeadPressGestureState();
    //private DeadMultipressGestureState deadMultiPressState = new DeadMultipressGestureState();

    ////Gesture Transitions
    //private GestureStateTransition toIdleState = new GestureStateTransition(GestureTransitionType.IDLE_TRANSITION, GestureStateType.IDLE_STATE);
    //private GestureStateTransition toPressState = new GestureStateTransition(GestureTransitionType.PRESS_TRANSITION, GestureStateType.PRESS_STATE);
    //private GestureStateTransition toMultipressState = new GestureStateTransition(GestureTransitionType.MULTIPRESS_TRANSITION, GestureStateType.MULTIPRESS_STATE);
    //private GestureStateTransition toDragState = new GestureStateTransition(GestureTransitionType.DRAG_TRANSITION, GestureStateType.DRAG_STATE);
    //private GestureStateTransition toDeadPressState = new GestureStateTransition(GestureTransitionType.DEAD_PRESS_TRANSITION, GestureStateType.DEAD_PRESS_STATE);
    //private GestureStateTransition toDeadMultipressState = new GestureStateTransition(GestureTransitionType.DEAD_MULTIPRESS_TRANSITION, GestureStateType.DEAD_MULTIPRESS_STATE);

    //private int activeTouchCount = 0;
    //public List<TouchData> HeldTouches { get; private set; } = new List<TouchData>();
    //public List<TouchData> NewTouches { get; private set; } = new List<TouchData>();

    //The maximum pixels a tap can be dragged to count as a tap.
    private float maxTapMovement = 50.0f;
    private float sqrMaxTapMovement;

    private Vector2 dragMovement;
    private Vector3 startMousePosition;

    private float dragMinTime = 0.1f;
    private float pinchMinTime = 0.1f;
    private float startTime;

    //If movement is greated than maxTapMovement, the tap failed.
    private bool tapFailed = false;
    private bool dragRecognized = false;
    private bool pinchRecognized = false;
    private bool middleMouseDrag = false;

    private void Start()
    {
        sqrMaxTapMovement = maxTapMovement * maxTapMovement;
        //InitStateMachine();
    }

    //private void InitStateMachine()
    //{
    //    LinkTransitions();
    //    AddStates();
    //    InputStateMachine.SetInitialStateID((int)GestureStateType.IDLE_STATE);
    //}

    //private void LinkTransitions()
    //{
    //    //Idle -> Press
    //    idleState.AddTransition(toPressState);

    //    //Press -> Idle
    //    //Press -> Multipress
    //    pressState.AddTransition(toIdleState);
    //    pressState.AddTransition(toMultipressState);

    //    //Deadpress -> Idle
    //    //Deadpress -> Multipress
    //    deadPressState.AddTransition(toIdleState);
    //    deadPressState.AddTransition(toMultipressState);

    //    //Multipress -> Idle
    //    //Multipress -> Press
    //    //Multipress -> DeadPress
    //    //Multipress -> DeadMultipress
    //    multipressState.AddTransition(toIdleState);
    //    multipressState.AddTransition(toPressState);
    //    multipressState.AddTransition(toDeadPressState);
    //    multipressState.AddTransition(toDeadMultipressState);

    //    //Dead Multipress -> Idle
    //    //Dead Multipress -> Multipress
    //    //Dead Multipress -> Dead Press
    //    deadMultiPressState.AddTransition(toIdleState);
    //    deadMultiPressState.AddTransition(toMultipressState);
    //    deadMultiPressState.AddTransition(toDeadPressState);
    //}

    //private void AddStates()
    //{
    //    InputStateMachine.AddState(idleState);
    //    InputStateMachine.AddState(pressState);
    //    InputStateMachine.AddState(multipressState);
    //    InputStateMachine.AddState(dragState);
    //    InputStateMachine.AddState(deadPressState);
    //    InputStateMachine.AddState(deadMultiPressState);
    //}

    private void Update()
    {
        //InputStateMachine.Update();
        TrackInput();
    }

    private void TrackInput()
    {
        if (!IsPointerOverUIObject())
        {
            CheckForMouseScroll();
        }

        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 2)
            {
                CheckForPinch();
            }

            CheckForDrag();
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
        {
            CheckForMouseDrag(0);
        }
        else if (Input.GetMouseButtonDown(2) || Input.GetMouseButton(2) || Input.GetMouseButtonUp(2))
        {
            CheckForMiddleMouseDrag();
        }
        else
        {
            dragRecognized = false;
            pinchRecognized = false;
            tapFailed = false;
            middleMouseDrag = false;
        }
    }

    private void CheckForMouseScroll()
    {
        //Update the bool variables too dragRecognized etc
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //Debug.Log("MOUSE SCROLL OCCURRED");
            EventAnnouncer.OnScrollOccurred?.Invoke(Input.GetAxis("Mouse ScrollWheel"));

            pinchRecognized = true;
            dragRecognized = false;
            tapFailed = true;
        }
    }

    private void CheckForMouseDrag(int button)
    {
        if (pinchRecognized) return;

        if (Input.GetMouseButtonDown(0))
        {
            dragMovement = Vector2.zero;
            startTime = Time.time;
            startMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            dragMovement = (Input.mousePosition - startMousePosition);

            if (!dragRecognized && Time.time - startTime > dragMinTime)
            {
                dragRecognized = true;
                pinchRecognized = false;
                tapFailed = true;

                //Debug.Log("MOUSE DRAG BEGAN");
                EventAnnouncer.OnDragBegan?.Invoke(Input.mousePosition);
            }
            else if (dragRecognized)
            {
                //Debug.Log("MOUSE DRAG HELD");
                EventAnnouncer.OnDragHeld?.Invoke(Input.mousePosition);
            }
            else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
            {
                tapFailed = true;
            }
        }
        else
        {
            if (dragRecognized)
            {
                //Debug.Log("MOUSE DRAG ENDED");
                EventAnnouncer.OnDragEnded?.Invoke(Input.mousePosition);
            }
            else
            {
                CheckForMouseTap();
            }
        }
    }

    private void CheckForMiddleMouseDrag()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragMovement = Vector2.zero;
            startTime = Time.time;
            startMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(2))
        {
            dragMovement = (Input.mousePosition - startMousePosition);

            if (!middleMouseDrag && Time.time - startTime > dragMinTime)
            {
                middleMouseDrag = true;

                //Debug.Log("MIDDLE MOUSE DRAG BEGAN");
                EventAnnouncer.OnMiddleMouseDragBegan?.Invoke(Input.mousePosition);
            }
            else if (middleMouseDrag)
            {
                //Debug.Log("MIDDLE MOUSE DRAG HELD");
                EventAnnouncer.OnMiddleMouseDragHeld?.Invoke(Input.mousePosition);
            }
            //else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
            //{
            //    tapFailed = true;
            //}
        }
        else
        {
            if (middleMouseDrag)
            {
                //Debug.Log("MIDDLE MOUSE DRAG ENDED");
                EventAnnouncer.OnMiddleMouseDragEnded?.Invoke(Input.mousePosition);
            }
        }
    }

    //First Priority
    private void CheckForPinch()
    {
        if (dragRecognized) return;

        Touch firstTouch = Input.touches[0];
        Touch secondTouch = Input.touches[1];

        if (firstTouch.phase == TouchPhase.Began && secondTouch.phase == TouchPhase.Began)
        {
            dragMovement = Vector2.zero;
            startTime = Time.time;
        }
        else if ((firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary) && (secondTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Stationary))
        {
            if (!pinchRecognized && Time.time - startTime > pinchMinTime)
            {
                pinchRecognized = true;
                dragRecognized = false;
                tapFailed = true;

                //Debug.Log("PINCH BEGAN");
                EventAnnouncer.OnPinchBegan?.Invoke(firstTouch, secondTouch);
            }
            else if (pinchRecognized)
            {
                //Debug.Log("PINCH HELD");
                EventAnnouncer.OnPinchHeld?.Invoke(firstTouch, secondTouch);
            }
            else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
            {
                tapFailed = true;
            }
        }
        else
        {
            if (pinchRecognized)
            {
                //Debug.Log("PINCH ENDED");
                EventAnnouncer.OnPinchEnded?.Invoke(firstTouch, secondTouch);
            }
        }
    }

    //Second Priority
    private void CheckForDrag()
    {
        if (pinchRecognized) return;

        Touch touch = Input.touches[0];
        //Debug.Log(touch.phase);

        if (touch.phase == TouchPhase.Began)
        {
            dragMovement = Vector2.zero;
            startTime = Time.time;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            dragMovement += touch.deltaPosition;

            if (!dragRecognized && Time.time - startTime > dragMinTime)
            {
                dragRecognized = true;
                pinchRecognized = false;
                tapFailed = true;

                //Debug.Log("DRAG BEGAN");
                EventAnnouncer.OnDragBegan?.Invoke(touch.position);
            }
            else if (dragRecognized)
            {
                //Debug.Log("DRAG HELD");
                EventAnnouncer.OnDragHeld?.Invoke(touch.position);
            }
            else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
            {
                tapFailed = true;
            }
        }
        else
        {
            if (dragRecognized)
            {
                //Debug.Log("DRAG ENDED");
                EventAnnouncer.OnDragEnded?.Invoke(touch.position);
            }
            else
            {
                CheckForTap();
            }
        }
    }

    //Last Priority
    private void CheckForTap()
    {
        if (dragRecognized) return;
        if (pinchRecognized) return;

        if (!tapFailed)
        {
            //Debug.Log("TAP OCCURRED");
            EventAnnouncer.OnTapOccurred?.Invoke(Input.touches[0].position);
        }
    }

    private void CheckForMouseTap()
    {
        if (dragRecognized) return;
        if (pinchRecognized) return;

        if (!tapFailed)
        {
            //Debug.Log("TAP OCCURRED");
            EventAnnouncer.OnTapOccurred?.Invoke(Input.mousePosition);
        }
    }

    //Found on https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html
    public static bool IsPointerOverUIObject()
    {
        //PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        //eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //List<RaycastResult> results = new List<RaycastResult>();
        //EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        //foreach (RaycastResult raycast in results)
        //{
        //    //Debug.Log("Raycast Layer: " + raycast.gameObject.layer.ToString());
        //    if (raycast.gameObject.layer == LayerMask.NameToLayer("UI"))
        //    {
        //        return true;
        //    }
        //}

        return false;
    }
}

//    private void TrackTouches()
//    {
//        //If we previously had touches and now don't, all touch ended
//        if (activeTouchCount != Input.touchCount && Input.touchCount == 0)
//        {
//            Debug.Log("TOUCH ENDED");
//            EventAnnouncer.OnTouchEnded?.Invoke();
//        }
//        //Iterate through the active touches and figure out their states
//        else
//        {
//            for (int i = 0; i < Input.touches.Length; ++i)
//            {
//                ref Touch t = ref Input.touches[i];
//                //Debug.Log("PHASE: " + t.phase);
//                //If the touch is new, fire the new touch event
//                if (t.phase == TouchPhase.Began)
//                {
//                    Debug.Log("TOUCH STARTED");
//                    EventAnnouncer.OnTouchStarted?.Invoke(t);
//                    NewTouches.Add(new TouchData(ref t));
//                }
//                //If the touch just ended or was interupted, fire the touch release event
//                else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
//                {
//                    Debug.Log("TOUCH RELEASED");
//                    EventAnnouncer.OnTouchReleased?.Invoke(t);

//                    for (int j = 0; j < HeldTouches.Count; ++j)
//                    {
//                        if (HeldTouches[j].Touch.fingerId == t.fingerId)
//                        {
//                            HeldTouches.RemoveAt(j);
//                            break;
//                        }
//                    }
//                }
//                //If the touch isn't starting or ending this frame and not already in the list, add touch to list
//                else
//                {
//                    bool isIn = false;
//                    foreach (TouchData td in HeldTouches)
//                    {
//                        if (td.Touch.fingerId == t.fingerId)
//                        {
//                            isIn = true;
//                        }
//                    }

//                    if (!isIn)
//                    {
//                        HeldTouches.Add(new TouchData(ref t));
//                    }
//                }
//            }

//            Debug.Log("Held Touch Count: " + HeldTouches.Count);

//            //If there are touches in the held list, fire the held touches event
//            if (HeldTouches.Count > 0)
//            {
//                bool isDrag = false;

//                for (int i = 0; i < HeldTouches.Count; ++i)
//                {
//                    TouchData td = HeldTouches[i];
//                    if (td.Touch.phase == TouchPhase.Moved)
//                    {
//                        td.Movement += td.Touch.deltaPosition;

//                        if (!td.DragRecognized && Time.time - td.StartTime > TouchData.MIN_DRAG_TIME)
//                        {
//                            td.DragRecognized = true;
//                            EventAnnouncer.OnDragStarted?.Invoke(td);
//                            Debug.Log("DRAG STARTED");
//                            isDrag = true;
//                        }
//                        else if (td.DragRecognized)
//                        {
//                            EventAnnouncer.OnDragContinued?.Invoke(td);
//                            Debug.Log("DRAG CONTINUED");
//                            isDrag = true;
//                        }
//                    }
//                    else if (td.DragRecognized)
//                    {
//                        EventAnnouncer.OnDragEnded?.Invoke(td);
//                        Debug.Log("DRAG ENDED");
//                        td.DragRecognized = false;
//                        isDrag = true;
//                    }
//                }

//                if (!isDrag)
//                {
//                    EventAnnouncer.OnTouchesHeld?.Invoke(HeldTouches.ToArray());
//                    Debug.Log("TOUCH HELD");
//                }
//            }

//            if (GestureTest.Instance != null)
//            {
//                GestureTest.Instance.DoUpdate();
//            }

//            //Reset the touches lists to prepare for the next frame
//            NewTouches.Clear();
//        }

//        //To keep track of the current frames touch count during the next frame
//        activeTouchCount = Input.touchCount;
//    }
//}

//public struct TouchData
//{
//    public const float MIN_DRAG_TIME = 0.1f;

//    //The maximum pixels a tap can be dragged to count as a tap.
//    public float sqrMinDragMovement;

//    public Touch Touch;
//    public float StartTime;
//    public Vector2 Movement;
//    public bool DragRecognized;

//    public TouchData(ref Touch t)
//    {
//        sqrMinDragMovement = 50.0f * 50.0f * Screen.dpi;
//        Touch = t;
//        StartTime = Time.time;
//        Movement = Vector2.zero;
//        DragRecognized = false;
//    }
//}
