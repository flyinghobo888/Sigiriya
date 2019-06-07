using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResize : MonoBehaviour
{
    public static int TARGET_SCREEN_DENSITY = 96;
    [SerializeField] private Vector2 nativeResolution = new Vector2(1080, 1920);
    private float scale = 1.0f;

    private Camera currentCamera;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        currentCamera = GetComponent<Camera>();
        CalculateCameraSize();
    }

    private void CalculateCameraSize()
    {
        if (currentCamera.orthographic)
        {
            scale = (Screen.height / nativeResolution.y);
            currentCamera.orthographicSize = (Screen.height / 2.0f) / scale;
        }
    }
}
