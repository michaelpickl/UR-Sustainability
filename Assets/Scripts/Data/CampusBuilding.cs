using System.Collections;
using System.Collections.Generic;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
using RTS_Cam;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;

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
    public Material renovationMaterial;  //renovationmaterial
    public Material oldMaterial;
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
        foreach(Measure measure in building.measures)
        {
            if(measure.done)
            {
                StartCoroutine(StartCollectingMoney(measure.name));
            }
        } 
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !constructionMode) //Building not clickable when in construction mode!
        {
            if(!sideMenuController.IsMenuOpen()) //can only click building when side menu is closed
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
        switch(measureName)
        {
            case "Papierkonzept verbessern":
                Transform dumpster = transform.Find("Tonnen");
                dumpster.gameObject.SetActive(false);
                break;
            case "E-Dienstfahrzeuge":
                Transform cars = transform.Find("Autos");
                cars.gameObject.SetActive(false);
                break;
            case "Energetische Gebäudesanierung":
                ApplyRenovationMaterial(renovationMaterial);
                break;
            default: break;
        }
    }

    public void HideMeasure(string measureName)
    {
        Transform child = transform.Find(measureName);
        if (child != null)
        {
            child.gameObject.SetActive(false);
        }
        switch(measureName)
        {
            case "Papierkonzept verbessern":
                Transform dumpster = transform.Find("Tonnen");
                dumpster.gameObject.SetActive(true);
                break;
            case "E-Dienstfahrzeuge":
                Transform cars = transform.Find("Autos");
                cars.gameObject.SetActive(true);
                break;
            case "Energetische Gebäudesanierung":
                ApplyRenovationMaterial(oldMaterial);
                break;
            default: break;
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
            yield return new WaitForSeconds(durationValue * 2.5f);
        }
        else
        {
            Debug.LogError("Ungültige Dauer: " + duration);
        }

        DeactivateConstructionMode();
        ShowMeasure(name);
        StartCoroutine(StartCollectingMoney(name));
    }

    public bool inConstructionMode(){
        return constructionMode;
    }

    public IEnumerator StartCollectingMoney(String measureName)
    {
        yield return new WaitForSeconds(30); //1 year!
        Transform child = transform.Find("MoneyCollection");
        if (child != null)
        {
            //child.gameObject.SetActive(true);
            MoneyCollectionButton moneyCollectionButton = child.GetComponent<MoneyCollectionButton>();
            moneyCollectionButton.ShowButton(buildingName, measureName);
        }
        else{
            print("MoneyCollection nicht gefunden!");
        }
        StartCoroutine(StartCollectingMoney(name));
    }

    public void ApplyRenovationMaterial(Material material)
    {
        //all renderer for the buildings in the gameobject
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        //check if material is asigned
        if (material != null)
        {
            //set material
            foreach (Renderer renderer in allRenderers)
            {
                if (renderer.gameObject.CompareTag("beton"))  // object must be tagged as "beton"
                {
                    Debug.Log("Applying material to: " + renderer.gameObject.name);
                    renderer.sharedMaterial = material;
                }
            }
            Debug.Log("Renovation material applied successfully to all buildings in the tract!");
        }
        else
        {
            Debug.LogError("Renovation material not assigned!");
        }
    }

}
