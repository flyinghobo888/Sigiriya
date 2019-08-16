using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPanel : MonoBehaviour
{
    private Transform buttonContainer;
    [SerializeField] private GameObject NavButton = null;

    private void Start()
    {
        buttonContainer = transform.Find("NavButtonContainer");

        for (int i = 0; i < (int)EnumLocation.SIZE; ++i)
        {
            EnumLocation location = (EnumLocation)i;
            GameObject button = Instantiate(NavButton, buttonContainer);
            button.GetComponent<Button>().onClick.AddListener(delegate
            {
                GoToLocation(location);
            });

            button.GetComponentInChildren<Text>().text = location.ToString();
        }

        ClosePanel();
    }

    public void GoToLocation(EnumLocation location)
    {
        if (LocationTracker.Instance.IsLocationRegistered(location))
        {
            EventAnnouncer.OnRequestLocationChange?.Invoke(LocationTracker.Instance.CurrentLocation, location, true);
            //Debug.Log("Attempt Success");
            ClosePanel();
        }
        else
        {
            //Debug.Log("Attempt Failed. Register location first!");
        }
    }


    public void TogglePanel()
    {
        if (gameObject.activeInHierarchy)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }


    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
