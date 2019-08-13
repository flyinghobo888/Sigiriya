using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private Parallax currentBackground;

    public void SetBackground(Parallax parallax)
    {
        currentBackground = parallax;
    }

    public void Scroll(float offset)
    {

    }
}
