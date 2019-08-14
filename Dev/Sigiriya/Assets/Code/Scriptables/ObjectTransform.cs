using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransform : ScriptableObject
{
    [SerializeField] public string Tag = "";
    [SerializeField] public Vector3 Position = Vector3.zero;
    [SerializeField] public Quaternion Rotation = Quaternion.identity;
    [SerializeField] public Vector3 Scale = new Vector3(1.0f, 1.0f, 1.0f);
    [SerializeField] public Vector2 AnchorPosition = Vector2.zero;
    [SerializeField] public Vector2 AnchorMin = Vector2.zero;
    [SerializeField] public Vector2 AnchorMax = Vector2.zero;
    [SerializeField] public Vector2 Pivot = new Vector2(0.5f, 0.5f);
    //[SerializeField] public Rect Rect = Rect.zero;

    public void Init(RectTransform transform)
    {
        Tag = transform.tag;
        Position = transform.localPosition;
        Rotation = transform.localRotation;
        Scale = transform.localScale;
        AnchorPosition = transform.anchoredPosition;
        AnchorMin = transform.anchorMin;
        AnchorMax = transform.anchorMax;
        Pivot = transform.pivot;
        //Rect = transform.rect;
    }

    public void ApplyTransform(RectTransform transform)
    {
        transform.tag = Tag;
        //transform.localPosition = Position;
        transform.localRotation = Rotation;
        //transform.localScale = Scale;
        //transform.anchoredPosition = AnchorPosition;
        transform.anchorMin = AnchorMin;
        transform.anchorMax = AnchorMax;
        transform.pivot = Pivot;
        //transform.rect. = Rect;
    }
}
