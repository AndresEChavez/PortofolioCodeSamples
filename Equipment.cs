using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    //Scripts communicating
    public NewCamera camfunctionScript;
    public Flashlight flashlightScript;
    public NewCamera camScript;
    //public FilterManager filterManagerScript;
    public PauseMenu pauseMenuScript;

    //gameObjects that are equiped
    private GameObject flashlight;
    private GameObject cameraObj;
    public GameObject flashlightCanvas;
    public Image cameraIcon;
    public Image flashlightIcon;
    public GameObject imagesIcon;
    public GameObject flashlightcanvas;
    private GameObject camerawithoutzoomcrosshair;

    public AudioManager SoundSFX;

    //colors
    Color selectedColor = new Color32(110, 209, 117, 255);
    Color nonSelectedColor = new Color32(190, 186, 186, 255);

    //190, 186, 186
    //.GetComponent<Image>().color = new Color(110, 209, 117); 

    // Start is called before the first frame update
    void Start()
    {
        SoundSFX = GetComponent<AudioManager>();
        imagesIcon = GameObject.Find("ImagesIconBG");
        cameraObj = transform.GetChild(0).GetChild(3).gameObject;
        flashlight = transform.GetChild(0).GetChild(2).gameObject;

        flashlight.SetActive(false);
        flashlightCanvas.SetActive(false);

        camerawithoutzoomcrosshair = GameObject.Find("camerawithoutzoomcrosshair");
        //filterManagerScript.FilterEquip = false;
        flashlightScript.CanUseFlashLight = false;

        camfunctionScript.cameraIsEquipped = true;
        camfunctionScript.cantuseCamera = false;

        imagesIcon.SetActive(false);
        cameraObj.SetActive(true);
        flashlight.SetActive(false);
        flashlightCanvas.SetActive(false);
        cameraIcon.color = selectedColor;
        flashlightIcon.color = nonSelectedColor;
        flashlightcanvas.SetActive(false);
        camerawithoutzoomcrosshair.SetActive(true);


    }

    // Update is called once per frame
    void Update()
    {
        equipped();

        if (camScript.capturedPhotoTaken == true)
        {
            cameraIcon.enabled = false;
            imagesIcon.SetActive(true);
            imagesIcon.GetComponent<Image>().color = selectedColor;
        }
    }

    public void equipped()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && camfunctionScript.cameraIsInUse == false && pauseMenuScript.isPaused == false && camfunctionScript.cameraIsEquipped == false)
        {
            SoundSFX.PlaySound(SoundSFX.MainSFX,SoundSFX.SwapingSFX);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && camfunctionScript.cameraIsInUse == false && pauseMenuScript.isPaused == false && flashlightScript.CanUseFlashLight == false)
        {
            SoundSFX.PlaySound(SoundSFX.MainSFX, SoundSFX.SwapingSFX);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && camfunctionScript.cameraIsInUse == false && pauseMenuScript.isPaused == false)
        {
            //filterManagerScript.FilterEquip = false;
            flashlightScript.CanUseFlashLight = false;

            camfunctionScript.cameraIsEquipped = true;
            camfunctionScript.cantuseCamera = false;

            cameraObj.SetActive(true);
            flashlight.SetActive(false);
            flashlightCanvas.SetActive(false);
            cameraIcon.color = selectedColor;
            flashlightIcon.color = nonSelectedColor;
            flashlightcanvas.SetActive(false);
            camerawithoutzoomcrosshair.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.Alpha2) && camfunctionScript.cameraIsInUse == false && pauseMenuScript.isPaused == false)
        {
            flashlight.SetActive(true);
            //filterManagerScript.FilterEquip = false;
            camfunctionScript.cantuseCamera = true;
            camfunctionScript.cameraIsEquipped = false;
            flashlightcanvas.SetActive(true);


            flashlightScript.CanUseFlashLight = true;

            cameraObj.SetActive(false);
            flashlightCanvas.SetActive(true);
            cameraIcon.color = nonSelectedColor;
            flashlightIcon.color = selectedColor;
            camerawithoutzoomcrosshair.SetActive(false);

            imagesIcon.SetActive(false);
            cameraIcon.enabled = true;
            imagesIcon.GetComponent<Image>().color = nonSelectedColor;

        }
    }

}

