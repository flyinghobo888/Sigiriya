using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDepthFromHeight : MonoBehaviour
{
    [SerializeField] private GameObject container = null;
    [SerializeField] private List<GameObject> affectedObjects = new List<GameObject>();
    [SerializeField] [Range(-0.0001f, -0.01f)] private float depthScaleOffset = -0.005f;
    [SerializeField] [Range(0.7f, 10.0f)] private float depthHeightOffset = 1.0f;
    [SerializeField] [Range(0.0f, 1.0f)]private float objectScaleOffset = 0.0f;
    private List<GameObject> orderedObjects = new List<GameObject>();
    private List<GameObject> unorderedObjects = new List<GameObject>();

    private void Start()
    {
        UpdateDepth();
    }

    private void OnValidate()
    {
        UpdateScale();
    }

    public void UpdateDepth()
    {
        if (container != null)
        {
            Sort();
        }
        else
        {
            //Debug.LogWarning("World container is null! Cannot sort.");
        }
    }

    //slow and gross, but if it's really a problem we can change it
    private void Sort()
    {
        orderedObjects.Clear();
        unorderedObjects.Clear();
        
        foreach (GameObject obj in affectedObjects)
        {
            unorderedObjects.Add(obj);
        }

        int minIndex;
        while (unorderedObjects.Count > 0)
        {
            minIndex = 0;
            for (int i = 0; i < unorderedObjects.Count; ++i)
            {
                if (unorderedObjects[i].GetComponent<RectTransform>().anchoredPosition.y < unorderedObjects[minIndex].GetComponent<RectTransform>().anchoredPosition.y)
                {
                    minIndex = i;
                }
            }

            orderedObjects.Add(unorderedObjects[minIndex]);
            unorderedObjects.RemoveAt(minIndex);
        }

        UpdateOrder();
        UpdateScale();
    }

    private void UpdateOrder()
    {
        foreach (GameObject obj in orderedObjects)
        {
            obj.transform.SetAsFirstSibling();
        }
    }

    private void UpdateScale()
    {
        foreach (GameObject obj in orderedObjects)
        {
            obj.transform.localScale = new Vector3(1, 1, 1) * obj.GetComponent<RectTransform>().anchoredPosition.y * (depthScaleOffset / depthHeightOffset);
            obj.transform.localScale += new Vector3(objectScaleOffset, objectScaleOffset, objectScaleOffset);
        }
    }
}
