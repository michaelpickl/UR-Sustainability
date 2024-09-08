using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
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

    void Start()
    {
        dataGetter = GameObject.Find("DataGetter").GetComponent<DataGetter>();
        rts_camera = GameObject.Find("RTS_Camera").GetComponent<Camera>();
        camManager = GameObject.Find("RTS_Camera").GetComponent<RTS_Camera>();
        cameraTarget = GameObject.Find("CameraTarget");
        building = dataGetter.GetBuilding(buildingName);
        if(building != null){
            print(building.consumers[0].type);
        }
        Vector3 targetPosition = CalculateTargetPosition();
        print(gameObject.name + " " + targetPosition.x);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                }
            }
        }
    }

    public Vector3 CalculateTargetPosition(float distanceFactor = 1.5f)
    {
        // Zugriff auf alle Renderer in diesem GameObject und seinen untergeordneten Objekten
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();

        if (renderers.Length > 0)
        {
            // Initialisiere die Bounds mit den ersten Renderer-Bounds
            Bounds combinedBounds = renderers[0].bounds;

            // Kombiniere die Bounds aller Renderer
            foreach (Renderer rend in renderers)
            {
                combinedBounds.Encapsulate(rend.bounds);
            }

            // Mittelpunkt des kombinierten Bounds (in Weltkoordinaten)
            Vector3 boundsCenter = combinedBounds.center;

            // Die größte Ausdehnung des kombinierten Bounds
            float boundsMaxSize = combinedBounds.extents.magnitude;

            // Berechne den Abstand basierend auf dem Field of View (FOV) der Kamera
            float fov = rts_camera.fieldOfView;
            float aspectRatio = rts_camera.aspect;

            // Berechnung des Abstands zur Kamera, um das Objekt vollständig anzuzeigen
            float distance = boundsMaxSize / Mathf.Sin(Mathf.Deg2Rad * fov / 2.0f);

            // Multipliziere den Abstand mit dem distanceFactor (für zusätzlichen Abstand)
            distance *= distanceFactor;

            // Neu: Die Kamera-Rotation berücksichtigen
            Vector3 cameraForward = rts_camera.transform.forward;
            Vector3 cameraRight = rts_camera.transform.right;
            Vector3 cameraUp = rts_camera.transform.up;

            print(cameraForward + " CAMERA FORWARD");
            print(boundsCenter + " BOUNDS CENTER");
            // Berechne die Zielposition, indem die Kamera entlang ihrer Vorwärtsachse zurückversetzt wird
            //Vector3 direction = rts_camera.transform.forward;
            //Vector3 targetPosition = boundsCenter - direction * distance;
            Vector3 targetPosition = boundsCenter - (cameraForward * distance);// - (cameraRight * distance);
            print(targetPosition + " TARGET POSITION");
            return targetPosition;
        }

        print("NO RENDERER FOUND");
        return rts_camera.transform.position; // Falls kein Renderer gefunden wurde, keine Änderung der Position
    }

}
