using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArPalcementManager : MonoBehaviour
{
    ARRaycastManager raycastManager;
    static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    public Camera ArCamera;
    public GameObject battleArena;
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        Vector3 screenCenter = new Vector3(Screen.width/2,Screen.height/2);
        Ray ray = ArCamera.ScreenPointToRay(screenCenter);
        
        if(raycastManager.Raycast(ray,raycastHits,TrackableType.PlaneWithinPolygon) )
        {
            Pose hitPose = raycastHits[0].pose;

            Vector3 positionToPlace = hitPose.position;

            battleArena.transform.position = positionToPlace;
        }
    }
}
