using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;

public class NewCamera : MonoBehaviour
{
    //public bool for reference in other scripts
    public bool cameraIsInUse;
    public bool cantuseCamera;
    public bool cameraIsEquipped;
    public bool photoTaken;
    public bool capturedPhotoTaken;

    //communicating to other scripts
    public CharacterMovement chrMovement;
    public PauseMenu pauseScript;
    public Equipment equipScript;

    //raycasts
    RaycastHit hit;
    Ray r;

    //CameraFuntions
    public GameObject removedObj;
    public GameObject cameraLens;
    private GameObject objcetCamera;
    public GameObject photo;
    public Camera internalCamera;


    private RenderTexture imageCap;
    int layerMask = 20;

    //Soundplay
    public AudioManager soundSFX;
    private GameObject camerawithoutzoomcrosshair;

    void Start()
    {
        equipScript = transform.GetComponentInParent<Equipment>();

        objcetCamera = transform.GetChild(3).gameObject;
        cameraLens.SetActive(false);
        cantuseCamera = true;
        photo.SetActive(false);
        photoTaken = false;
        removedObj = null;
        soundSFX = GameObject.Find("Player").GetComponent<AudioManager>();
        internalCamera.enabled = true;
        camerawithoutzoomcrosshair = GameObject.Find("camerawithoutzoomcrosshair");

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
            photo.SetActive(false);
            camerawithoutzoomcrosshair.SetActive(false);
            soundSFX.PlaySound(soundSFX.MainSFX, soundSFX.ZoomInSFX);

        }
        if (cameraIsEquipped == true && Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 60;
            if (capturedPhotoTaken == false)
            {
                objcetCamera.SetActive(true);
            }
            cameraLens.SetActive(false);
            chrMovement.speed = 5;
            cameraIsInUse = false;
            camerawithoutzoomcrosshair.SetActive(true);

            if (photoTaken == true)
            {
                photo.SetActive(true);
                internalCamera.enabled = false;
            }
            soundSFX.MainSFX.Stop();
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
                internalCamera.enabled = true;
                soundSFX.PlaySound(soundSFX.MainSFX, soundSFX.CameraShutterSFX);
                imageCap = internalCamera.targetTexture;
                Renderer rend = photo.GetComponent<Renderer>();
                rend.material.mainTexture = imageCap;
                photoTaken = true;

                if (hit.transform.tag == "Capture" || hit.transform.tag == "Enemy")
                {
                    removedObj = hit.collider.gameObject;
                    removedObj.gameObject.layer = layerMask;
                    capturedPhotoTaken = true;
                    objcetCamera.SetActive(false);

                    StartCoroutine("holdForPic");
                    soundSFX.PlaySound(soundSFX.SecondSFX, soundSFX.CaptureSFX);
                }

                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<AI_Agent>().canFollow = false;
                }
            }

        }

    }
    IEnumerator holdForPic()
    {
        yield return new WaitForSeconds(0.1f);
        removedObj.SetActive(false);
        cantuseCamera = true;
        removedObj.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        internalCamera.enabled = false;
    }


    private void ObjectSpawning()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

        Vector3 wordPos;

        Ray ray = Camera.main.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Q) &&  removedObj != null)
        {
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                wordPos = hit.point + (hit.normal * removedObj.transform.localScale.z) / 2;

                Debug.Log(hit.transform.name);

                if (hit.transform.name.Contains("EdgeX-"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y +.5f, hit.point.z);
                }

                if (hit.transform.name.Contains("EdgeX+"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y + .5f, hit.point.z);
                }

                if (hit.transform.name.Contains("EdgeZ-"))
                {
                    wordPos = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z - .5f);
                }

                if (hit.transform.name.Contains("EdgeZ+"))
                {
                    wordPos = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("EdgeXC-"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y - .5f, hit.point.z);
                }

                if (hit.transform.name.Contains("EdgeXC+"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y - .5f, hit.point.z);
                }

                if (hit.transform.name.Contains("EdgeZC-"))
                {
                    wordPos = new Vector3(hit.point.x, hit.point.y - .5f, hit.point.z - .5f);
                }

                if (hit.transform.name.Contains("EdgeZC+"))
                {
                    wordPos = new Vector3(hit.point.x, hit.point.y - .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("CornerX-"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y + .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("CornerX+"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y + .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("CornerZ-"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y + .5f, hit.point.z -.5f);
                }

                if (hit.transform.name.Contains("CornerZ+"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y + .5f, hit.point.z - .5f);
                }

                if (hit.transform.name.Contains("CornerXC-"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y - .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("CornerXC+"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y - .5f, hit.point.z + .5f);
                }

                if (hit.transform.name.Contains("CornerZC-"))
                {
                    wordPos = new Vector3(hit.point.x + .5f, hit.point.y - .5f, hit.point.z - .5f);
                }

                if (hit.transform.name.Contains("CornerZC+"))
                {
                    wordPos = new Vector3(hit.point.x - .5f, hit.point.y - .5f, hit.point.z - .5f);
                }
            }
            else
            {
                wordPos = Camera.main.ScreenToWorldPoint(mousePos);
            }

            Vector3 camForward;
            camForward = Camera.main.transform.TransformDirection(transform.forward);
            camForward.z -= removedObj.transform.localScale.z;

            removedObj.transform.position = wordPos;

            removedObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            soundSFX.PlaySound(soundSFX.MainSFX, soundSFX.ObjSpawnSFX);
            removedObj.SetActive(true);


            if (removedObj.tag == "Enemy")
            {
                wordPos = hit.point + (hit.normal * removedObj.transform.localScale.z) / 2;
                removedObj.GetComponent<AI_Agent>().canFollow = true;
                removedObj = null;
            }
            else
            {
                removedObj = null;
            }

            capturedPhotoTaken = false;
            if (cameraIsEquipped == true)
            {
                objcetCamera.SetActive(true);
            }
            
            photo.SetActive(false);
            photoTaken = false;
            equipScript.cameraIcon.enabled = true;
            equipScript.imagesIcon.SetActive(false);
        }


        if (removedObj == null)
        {
            cantuseCamera = false;
        }
        else
        {
            cantuseCamera = true;
        }
    }
}