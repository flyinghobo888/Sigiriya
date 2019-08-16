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

    private Vector3 startDrag, offsetDir, offsetVecFG, offsetVecMG, offsetVecBG;

	public bool canClamp = true;

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
        ResetParallax();

        parallax = newBackground;

        if (parallax)
        {
			Background.transform.localScale = new Vector3(parallax.BackgroundScale, 1, 1);
			Midground.transform.localScale = new Vector3(parallax.MidgroundScale, 1, 1);
			Foreground.transform.localScale = new Vector3(parallax.ForegroundScale, 1, 1);

			Background.gameObject.SetActive(true);
            Midground.gameObject.SetActive(true);
            Foreground.gameObject.SetActive(true);

            Background.sprite = parallax.Background;
            Midground.sprite = parallax.Midground;
            Foreground.sprite = parallax.Foreground;

            Background.SetNativeSize();
            Midground.SetNativeSize();
            Foreground.SetNativeSize();

            if (!Background.sprite) Background.gameObject.SetActive(false);
            if (!Midground.sprite) Midground.gameObject.SetActive(false);
            if (!Foreground.sprite) Foreground.gameObject.SetActive(false);
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
		//offsetVec.x += offsetDelta.x;

		if (parallax == null)
        {
            Debug.LogError("Add a parallax option to the current location!");
            return;
        }

		currentLocation.FG.localPosition = Clamp(Foreground, currentLocation.FG.localPosition, offsetDelta, parallax.ForegroundSpeedMult, parallax.ForegroundScale, ref offsetVecFG);
		if (canClamp)
		{
			currentLocation.MG.localPosition = Clamp(Midground, currentLocation.MG.localPosition, offsetDelta, parallax.MidgroundSpeedMult, parallax.MidgroundScale, ref offsetVecMG);
			currentLocation.BG.localPosition = Clamp(Background, currentLocation.BG.localPosition, offsetDelta, parallax.BackgroundSpeedMult, parallax.BackgroundScale, ref offsetVecBG);

		}
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

	private Vector3 Clamp(Image img, Vector3 containerPosition, Vector3 offsetDelta, float mult, float scale, ref Vector3 offsetVec)
    {
		//Get the direction of movement
		int direction = 0;
		if (offsetDelta.x != 0)
		{
			direction = (int)(offsetDelta.x / Mathf.Abs(offsetDelta.x));
		}

		Vector3 clampedPos = Vector3.zero;// = offsetVec.x + offsetDelta.x * mult;
		clampedPos.x = offsetVec.x + offsetDelta.x * mult;


		if (direction > 0) //right
		{
			if (img.rectTransform.rect.xMin * scale + clampedPos.x >= parentRect.rect.xMin) // if true, DO NOT move
			{
				Debug.Log("Out of right bound");
				offsetVec.x = parentRect.rect.xMin - img.rectTransform.rect.xMin * scale;

				canClamp = false;
				return offsetVec;
			}

			//offsetVec.x += offsetDelta.x;
		}
		else if (direction < 0) //left
		{
			if (img.rectTransform.rect.xMax * scale + clampedPos.x <= parentRect.rect.xMax)
			{
				Debug.Log("Out of left bound");
				offsetVec.x = parentRect.rect.xMax - img.rectTransform.rect.xMax * scale;

				canClamp = false;
				return offsetVec;
			}
		}
		else if (direction == 0)
		{
			//Debug.Log("Not")
			offsetVec.x -= offsetDelta.x;

			canClamp = false;
			return containerPosition;
		}

		offsetVec.x = clampedPos.x;

		canClamp = true;
		return clampedPos;
    }

    private void ResetParallax()
    {
        StartCoroutine(ResetBackground());
    }

    private IEnumerator ResetBackground()
    {
        yield return null;

        if (currentLocation)
        {
            currentLocation.BG.localPosition = Vector3.zero;
            currentLocation.MG.localPosition = Vector3.zero;
            currentLocation.FG.localPosition = Vector3.zero;
        }

        Background.rectTransform.localPosition = Vector3.zero;
        Midground.rectTransform.localPosition = Vector3.zero;
        Foreground.rectTransform.localPosition = Vector3.zero;
        offsetVecFG = Vector3.zero;
		offsetVecMG = Vector3.zero;
		offsetVecBG = Vector3.zero;
	}
}
