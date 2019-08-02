using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientChanger : ManagerBase<GradientChanger>
{
    [SerializeField] private Image topGradient;
    [SerializeField] private Image bottomGradient;
    [Space]
    [SerializeField] [Range(0, 1)] private float masterOpacity;
    [Space]
    [SerializeField] private Color topSunrise;
    [SerializeField] private Color bottomSunrise;
    [SerializeField] [Range(0, 1)] private float sunriseOpacity;
    [Space]
    [SerializeField] private Color topMorning;
    [SerializeField] private Color bottomMorning;
    [SerializeField] [Range(0, 1)] private float morningOpacity;
    [Space]
    [SerializeField] private Color topMidday;
    [SerializeField] private Color bottomMidday;
    [SerializeField] [Range(0, 1)] private float middayOpacity;
    [Space]
    [SerializeField] private Color topEvening;
    [SerializeField] private Color bottomEvening;
    [SerializeField] [Range(0, 1)] private float eveningOpacity;
    [Space]
    [SerializeField] private Color topNight;
    [SerializeField] private Color bottomNight;
    [SerializeField] [Range(0, 1)] private float nightOpacity;
    [Space]
    [SerializeField] private EnumTime previewTime;

    private Dictionary<EnumTime, ColorData> timeColors = new Dictionary<EnumTime, ColorData>();

    private Color topStartColor, topEndColor;
    private Color bottomStartColor, bottomEndColor;
    [Range(0, 1)] private float startOpacity, endOpacity;

    private Color topColor, bottomColor;

    private GlobalTimeTracker timeTracker;
    private float lerpAlpha;

    private void OnEnable()
    {
        timeTracker = GlobalTimeTracker.Instance;

        EventAnnouncer.OnTimeChanged += UpdateLerpAnchors;

        timeColors.Add(EnumTime.SIZE, new ColorData(topSunrise, bottomSunrise, sunriseOpacity));
        timeColors.Add(EnumTime.SUNRISE, new ColorData(topSunrise, bottomSunrise, sunriseOpacity));
        timeColors.Add(EnumTime.MORNING, new ColorData(topMorning, bottomMorning, morningOpacity));
        timeColors.Add(EnumTime.MIDDAY, new ColorData(topMidday, bottomMidday, middayOpacity));
        timeColors.Add(EnumTime.EVENING, new ColorData(topEvening, bottomEvening, eveningOpacity));
        timeColors.Add(EnumTime.NIGHT, new ColorData(topNight, bottomNight, nightOpacity));

        UpdateLerpAnchors(timeTracker.CurrentTimeOfDay);
    }

    private void OnDisable()
    {
        EventAnnouncer.OnTimeChanged -= UpdateLerpAnchors;
    }

    private void OnValidate()
    {
        ChangeGradient(previewTime);
    }

    private void Update()
    {
        LerpColors();
    }

    private void LerpColors()
    {
        topColor = Color.Lerp(topStartColor, topEndColor, timeTracker.TimeAlpha);
        topColor.a *= Mathf.Lerp(startOpacity, endOpacity, timeTracker.TimeAlpha);

        bottomColor = Color.Lerp(bottomStartColor, bottomEndColor, timeTracker.TimeAlpha);
        bottomColor.a *= Mathf.Lerp(startOpacity, endOpacity, timeTracker.TimeAlpha);

        topColor.a *= masterOpacity;
        bottomColor.a *= masterOpacity;

        topGradient.color = topColor;
        bottomGradient.color = bottomColor;
    }

    private void UpdateLerpAnchors(EnumTime newTime)
    {
        LerpColors();

        if (timeColors.TryGetValue(newTime, out ColorData currentColorData))
        {
            if (timeColors.TryGetValue((EnumTime)(((int)newTime + 1) % (int)EnumTime.SIZE), out ColorData nextColorData))
            {
                topStartColor = currentColorData.TopColor;
                topEndColor = nextColorData.TopColor;

                bottomStartColor = currentColorData.BottomColor;
                bottomEndColor = nextColorData.BottomColor;

                startOpacity = currentColorData.Opacity;
                endOpacity = nextColorData.Opacity;

                //previewTime = newTime;
            }
        }
    }

    public void ChangeGradient(EnumTime time)
    {
        topColor = topGradient.color;
        bottomColor = bottomGradient.color;

        switch (time)
        {
            default:
            case EnumTime.SUNRISE:
                ChangeColor(ref topColor, topSunrise, ref bottomColor, bottomSunrise, sunriseOpacity);
                break;
            case EnumTime.MORNING:
                ChangeColor(ref topColor, topMorning, ref bottomColor, bottomMorning, morningOpacity);
                break;
            case EnumTime.MIDDAY:
                ChangeColor(ref topColor, topMidday, ref bottomColor, bottomMidday, middayOpacity);
                break;
            case EnumTime.EVENING:
                ChangeColor(ref topColor, topEvening, ref bottomColor, bottomEvening, eveningOpacity);
                break;
            case EnumTime.NIGHT:
                ChangeColor(ref topColor, topNight, ref bottomColor, bottomNight, nightOpacity);
                break;
        }

        topColor.a *= masterOpacity;
        bottomColor.a *= masterOpacity;

        topGradient.color = topColor;
        bottomGradient.color = bottomColor;
    }

    private void ChangeColor(ref Color topColor, Color newTopColor, 
        ref Color bottomColor, Color newBottomColor, float opacity)
    {
        topColor = newTopColor;
        bottomColor = newBottomColor;
        topColor.a *= opacity;
        bottomColor.a *= opacity;
    }

    private struct ColorData
    {
        public Color TopColor;
        public Color BottomColor;
        [Range(0, 1)] public float Opacity;

        public ColorData(Color topColor, Color bottomColor, float opacity)
        {
            TopColor = topColor;
            BottomColor = bottomColor;
            Opacity = opacity;
        }
    }
}
