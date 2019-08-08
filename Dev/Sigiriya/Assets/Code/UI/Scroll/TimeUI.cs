using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : MonoBehaviour
{
    [SerializeField] private Image prevTimeImage = null;
    [SerializeField] private Image currentTimeImage = null;

    [SerializeField] private Sprite background = null;
    [SerializeField] private Sprite sunrise = null;
    [SerializeField] private Sprite morning = null;
    [SerializeField] private Sprite midday = null;
    [SerializeField] private Sprite evening = null;
    [SerializeField] private Sprite night = null;

    private Color colorVar = new Color();

    private Dictionary<EnumDisplayTime, Sprite> sprites = new Dictionary<EnumDisplayTime, Sprite>();

    private void Start()
    {
        InitImages();

        UpdateUI(GlobalTimeTracker.Instance.ExternalTimeOfDay, GlobalTimeTracker.Instance.ExternalTimeOfDay, 1.0f);
    }

    private void OnEnable()
    {
        EventAnnouncer.OnTimeTransitioning += UpdateUI;
        UpdateUI(GlobalTimeTracker.Instance.ExternalTimeOfDay, GlobalTimeTracker.Instance.ExternalTimeOfDay, 1.0f);
    }

    private void OnDisable()
    {
        EventAnnouncer.OnTimeTransitioning -= UpdateUI;
    }

    private void UpdateUI(EnumDisplayTime prevTime, EnumDisplayTime currentTime, float alpha)
    {
        sprites.TryGetValue(prevTime, out Sprite prevTimeSprite);
        sprites.TryGetValue(currentTime, out Sprite currentTimeSprite);

        SetImage(prevTimeImage, prevTimeSprite, 1.0f - alpha);
        SetImage(currentTimeImage, currentTimeSprite, alpha);
    }

    private void InitImages()
    {
        sprites.Clear();
        sprites.Add(EnumDisplayTime.SUNRISE, sunrise);
        sprites.Add(EnumDisplayTime.MORNING, morning);
        sprites.Add(EnumDisplayTime.MIDDAY, midday);
        sprites.Add(EnumDisplayTime.EVENING, evening);
        sprites.Add(EnumDisplayTime.NIGHT, night);
    }

    private void SetImage(Image image, Sprite sprite, float alpha)
    {
        image.sprite = sprite;
        colorVar = image.color;
        colorVar.a = alpha;
        image.color = colorVar;
    }
}
