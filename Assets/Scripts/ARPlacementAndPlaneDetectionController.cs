using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
 

public class ARPlacementAndPlaneDetectionController : MonoBehaviour
{
    ARPlaneManager aRPlaneManager;
    ArPalcementManager PlacementManager;

    public GameObject placeButton;
    public GameObject adjustButton;
    public GameObject SearchButton;
    public TextMeshProUGUI informUIPaneltext;

    private void Awake()
    {
        aRPlaneManager = GetComponent<ARPlaneManager>();
        PlacementManager = GetComponent<ArPalcementManager>();
    }
    void Start()
    {
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        SearchButton.SetActive(false);
        informUIPaneltext.text = "Move Phone To Adjust The Plane for Battle Arena";
    }

    void Update()
    {
        
    }

    private void actiaveOrDeactivatePlane(bool x)
    {
        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(x);
        }
    }

    public void DisablePlaneAndPlacementDetection()
    {
        aRPlaneManager.enabled = false;
        PlacementManager.enabled = false;

        actiaveOrDeactivatePlane(false);
        placeButton.SetActive(false);
        adjustButton.SetActive(true);
        SearchButton.SetActive(true);

        informUIPaneltext.text = "Arena Palced, Search For Games";

    }
    public void EnablePlaneAndPlacementDetection()
    {
        aRPlaneManager.enabled = true;
        PlacementManager.enabled = true;

        actiaveOrDeactivatePlane(true);
        placeButton.SetActive(true);
        adjustButton.SetActive(false);
        SearchButton.SetActive(false);
        informUIPaneltext.text = "Move Phone To Adjust The Plane for Battle Arena";

    }
}
