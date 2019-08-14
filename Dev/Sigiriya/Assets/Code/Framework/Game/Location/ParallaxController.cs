using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private RectTransform parentRect = null;

    [SerializeField] private Image Background = null;
    [SerializeField] private Image Midground = null;
    [SerializeField] private Image Foreground = null;

    private LocationController currentLocation;
    private Parallax parallax;

    private Vector3 startDrag, offsetDir, offsetVec;

    private void OnEnable()
    {
        EventAnnouncer.OnDragBegan += SetStartPos;
        EventAnnouncer.OnDragHeld += SetDirAndMove;
        EventAnnouncer.OnDragEnded += SetDirAndMove;
    }

    private void OnDisable()
    {
        EventAnnouncer.OnDragHeld -= MoveScreen;
    }

    public void SetBackground(Parallax newBackground)
    {
        currentLocation = LocationTracker.Instance.GetLocationController(LocationTracker.Instance.CurrentLocation);

        parallax = newBackground;

        if (parallax)
        {
            Background.sprite = parallax.Background;
            Midground.sprite = parallax.Midground;
            Foreground.sprite = parallax.Foreground;

            Background.SetNativeSize();
            Midground.SetNativeSize();
            Foreground.SetNativeSize();
        }
        else
        {
            Debug.LogWarning("Parallax was null!");
        }
    }

    private void SetStartPos(Vector3 start)
    {
        startDrag = start;
    }

    private void SetDirAndMove(Vector3 endPos)
    {
        offsetDir = endPos - startDrag;
        startDrag = endPos;

        MoveScreen(offsetDir);
    }

    public void MoveScreen(Vector3 offsetDelta)
    {
        offsetVec.x += offsetDelta.x;

        currentLocation.BG.localPosition = Clamp(Background, offsetVec * parallax.BackgroundSpeedMult);
        currentLocation.MG.localPosition = Clamp(Midground, offsetVec * parallax.MidgroundSpeedMult);
        currentLocation.FG.localPosition = Clamp(Foreground, offsetVec * parallax.ForegroundSpeedMult);
    }

    private bool CheckBounds(Image img, Vector3 offset)
    {
        if ((((img.rectTransform.localPosition.x + offset.x) - (img.rectTransform.rect.width / 2)) >
            parentRect.localPosition.x - (parentRect.rect.width / 2)) ||
            (((img.rectTransform.localPosition.x + offset.x) + (img.rectTransform.rect.width / 2)) <
            parentRect.localPosition.x + (parentRect.rect.width / 2)))
        {
            return false;
        }

        return true;
    }

    private Vector3 Clamp(Image img, Vector3 offset)
    {
        Vector3 clampedPos = offset;/*img.rectTransform.localPosition*/;
        //clampedPos.x = dragDir.x;
        //clampedPos.x = Mathf.Clamp(clampedPos.x, boundary.localPosition.x - boundary.rect.width / 2, boundary.localPosition.x + boundary.rect.width / 2);
        return clampedPos;
    }
}
