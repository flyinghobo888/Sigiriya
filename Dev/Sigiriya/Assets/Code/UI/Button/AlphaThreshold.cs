using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaThreshold : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] private float alphaThreshold = 0.005f;
    private Image buttonImage;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.alphaHitTestMinimumThreshold = alphaThreshold;
    }
}
