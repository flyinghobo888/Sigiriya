using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class LocationController : MonoBehaviour
{
    [SerializeField] private Location locationData;
    public Location LocationData { get => locationData; private set { locationData = value; } }
    //ParallaxController BackgroundParallax { get; private set; }

    //Get the random images based on flags and do parallax stuff in here when we get there.
}
