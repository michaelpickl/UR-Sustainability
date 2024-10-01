using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using RTS_Cam;

public class CampusBuilding : MonoBehaviour
{
    private DataGetter dataGetter;
    public string buildingName;
    private Building building;
    private Camera rts_camera;
    private RTS_Camera camManager;
    private GameObject cameraTarget;
    private Converter converter;

    public SideMenuController sideMenuController; 
    private bool constructionMode;
    

    void Start()
    {
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        rts_camera = GameObject.Find("RTS_Camera").GetComponent<Camera>();
        camManager = GameObject.Find("RTS_Camera").GetComponent<RTS_Camera>();
        converter = GameObject.Find("Converter").GetComponent<Converter>();
        cameraTarget = GameObject.Find("CameraTarget");
        building = dataGetter.GetBuilding(buildingName);

        if(sideMenuController == null)
        {
            sideMenuController = GameObject.FindObjectOfType<SideMenuController>();
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !constructionMode) //Building not clickable when in construction mode!
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedObject = hit.transform.gameObject;
                if(clickedObject.name == gameObject.name)
                {
                    Vector3 targetPosition = CalculateTargetPosition();
                    cameraTarget.transform.position = targetPosition;
                    camManager.SetTarget(cameraTarget.transform);
                    Debug.Log("Clicked on: " + clickedObject.name);

                    if(sideMenuController != null)
                    sideMenuController.OpenMenuWithBuildingName(buildingName);
                }
            }
        }
    }

    public Vector3 CalculateTargetPosition(float distanceFactor = 1.5f)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            Bounds combinedBounds = renderers[0].bounds;
            foreach (Renderer rend in renderers)
            {
                combinedBounds.Encapsulate(rend.bounds);
            }
            Vector3 boundsCenter = combinedBounds.center;
            float boundsMaxSize = combinedBounds.extents.magnitude;

            float fov = rts_camera.fieldOfView;
            float aspectRatio = rts_camera.aspect;

            float distance = boundsMaxSize / Mathf.Sin(Mathf.Deg2Rad * fov / 2.0f);
            distance *= distanceFactor;

            Vector3 direction = rts_camera.transform.forward;
            Vector3 targetPosition = boundsCenter - (direction * distance);
            //print(targetPosition + " TARGET POSITION");
            return targetPosition;
        }

        print("NO RENDERER FOUND");
        return rts_camera.transform.position; 
    }

    public void ShowMeasure(string measureName)
    {
        Transform child = transform.Find(measureName);
        if (child != null)
        {
            child.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"{measureName} konnte nicht gefunden werden.");
        }
    }

    public void HideMeasure(string measureName)
    {
        Transform child = transform.Find(measureName);
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning($"{measureName} konnte nicht gefunden werden.");
        }
    }

    public void ActivateConstructionMode()
    {
        Transform child = transform.Find("Baumodus");
        if (child != null)
        {
            child.gameObject.SetActive(true);
        }
        else{
            print("Baumodus NICHT GEFUNDEN");
        }
        constructionMode = true;
    }

    public void DeactivateConstructionMode()
    {
        Transform child = transform.Find("Baumodus");
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
        constructionMode = false;
    }

    public IEnumerator StartConstruction(string duration, string name)
    {
        ActivateConstructionMode();
        
        // Konvertiere die String-Dauer in einen float-Wert
        float durationValue;
        if (float.TryParse(duration, out durationValue))
        {
            yield return new WaitForSeconds(durationValue * 10);
        }
        else
        {
            Debug.LogError("Ungültige Dauer: " + duration);
        }

        DeactivateConstructionMode();
        ShowMeasure(name);
    }

    public bool inConstructionMode(){
        return constructionMode;
    }

}
