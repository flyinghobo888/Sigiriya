using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class NavigationPanel : MonoBehaviour
{
    Transform buttonContainer;

    [SerializeField]
    GameObject NavButton;
    void Start()
    {
        buttonContainer = transform.Find ("NavButtonContainer");
        foreach (EnumLocation l in (EnumLocation []) System.Enum.GetValues (typeof (EnumLocation)))
        {
            if (l != EnumLocation.SIZE)
            {
                GameObject g = Instantiate (NavButton, buttonContainer);
                g.GetComponent<Button> ().onClick.AddListener (delegate
                {
                    GoToLocation (l.ToString ());
                });
                g.GetComponentInChildren<Text> ().text = l.ToString ();
            }
        }

        ClosePanel ();
    }

    public void GoToLocation(string locationName)
    {
        EnumLocation tempLocation;
        Debug.Log ("Attempt go to " + locationName);
        if (System.Enum.TryParse (locationName, out tempLocation))
        {
            EventAnnouncer.OnRequestLocationChange?.Invoke (tempLocation, true);
            Debug.Log ("Attempt Success");
            ClosePanel ();
        }
        else
        {
            Debug.Log ("Attempt Fail");
        }
    }


    public void TogglePanel()
    {
        if (gameObject.activeInHierarchy)
            ClosePanel ();
        else
            OpenPanel ();
    }

    public void OpenPanel()
    {
        gameObject.SetActive (true);
    }


    public void ClosePanel()
    {
        gameObject.SetActive (false);
    }

}
