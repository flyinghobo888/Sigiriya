using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNavigation : MonoBehaviour
{
    public void GoToHome(bool shouldFade)
    {
        Debug.Log ("GO TO HOME");
        EventAnnouncer.OnRequestLocationChange?.Invoke (EnumLocation.HOME, shouldFade);
    }

    public void GoToSpring(bool shouldFade)
    {
        Debug.Log ("GO TO SPRING");
        EventAnnouncer.OnRequestLocationChange?.Invoke (EnumLocation.SPRING, shouldFade);
    }

}
