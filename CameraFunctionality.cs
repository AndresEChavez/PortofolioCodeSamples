using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CameraFunctionality : MonoBehaviour
{
    //public bool for reference in other scripts
    public bool cameraIsInUse;
    public bool cantuseCamera;
    public bool cameraIsEquipped;

    //public objects in this script
    public GameObject objcetCamera;
    public GameObject cameraLens;
    public GameObject theCube;
    public GameObject theWall;
    public GameObject photo;


    //pictures
    public Camera internalCamera;
    private bool photoToggle;

    //communicating to other scripts
    public CharacterMovement chrMovement;
    public PauseMenu pauseScript;

    //raycasts
    RaycastHit hit;
    Ray r;

    //**STORAGE** Here we will store things we delete with the cam
    public GameObject deletedObj;
    public string deletedObjName;

    // Start is called before the first frame update
    void Start()
    {
        cameraLens.SetActive(false);
        cantuseCamera = true;

        photo.SetActive(false);
        internalCamera.enabled = true;

        deletedObjName = "empty";
    }

    // Update is called once per frame
    void Update()
    {
        cameraZoom();
        cameraRay();
        ObjectSpawning();

    }

    private void cameraZoom()
    {
        if (cameraIsEquipped == true && Input.GetMouseButtonDown(1) && pauseScript.isPaused == false)
        {
            Camera.main.fieldOfView = 40;
            objcetCamera.SetActive(false);
            cameraLens.SetActive(true);
            chrMovement.speed = 1.5f;
            cameraIsInUse = true;
            internalCamera.enabled = true;
        }
        if (cameraIsEquipped == true && Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 60;
            objcetCamera.SetActive(true);
            cameraLens.SetActive(false);
            chrMovement.speed = 5;
            cameraIsInUse = false;
            internalCamera.enabled = false;
        }
    }

    private void cameraRay()
    {
        r.origin = gameObject.transform.position;
        r.direction = gameObject.transform.forward;

        if (cantuseCamera == false)
        {
            if (Physics.Raycast(r, out hit, 100) && cameraIsInUse == true && Input.GetMouseButtonDown(0))
            {
                internalCamera.enabled = false;
                photo.SetActive(true);

                if (hit.transform.tag == "Capture")
                {
                    deletedObj = hit.collider.gameObject;
                    deletedObjName = hit.collider.gameObject.name;
                    deletedObj.SetActive(false);
                    cantuseCamera = true;

                    photo.SetActive(true);
                }
            }
        }

        //Debug.DrawLine(r.origin, r.origin + r.direction * 100, Color.magenta);
    }

    private void ObjectSpawning()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        Vector3 wordPos;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            wordPos = hit.point;
        }
        else
        {
            wordPos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        if (Input.GetKeyDown(KeyCode.Q) && deletedObjName.Contains("Cube") && cameraIsEquipped == true)
        {
            Instantiate(theCube, wordPos, Quaternion.identity);
            deletedObjName = "empty";
        }

        if (Input.GetKeyDown(KeyCode.Q) && deletedObjName.Contains("Wall") && cameraIsEquipped == true)
        {
            Instantiate(theWall, wordPos, Quaternion.identity);
            deletedObjName = "empty";
        }

        if (deletedObjName == "empty")
        {
            cantuseCamera = false;
        }
        else
        {
            cantuseCamera = true;
        }
    }

    private void AutoOff()
    {
        if (internalCamera.enabled == true)
        {
            internalCamera.enabled = false;
        }

    }

}
