using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Flashlight : MonoBehaviour
{

    private bool canUseFlashLight;

    RaycastHit hit;
    Ray r;
    public Light fLight;

    Rigidbody rbg;
    public PauseMenu pauseScript;

    private GameObject DamageTrigger;

    public AudioManager soundSFX;

    public bool CanUseFlashLight {
        get => canUseFlashLight;
        set => canUseFlashLight = value;
    }

    void Awake()
    {
        CanUseFlashLight = false;
        DamageTrigger = GameObject.Find("DamageTrigger");
        soundSFX = GameObject.Find("Player").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && CanUseFlashLight == true && pauseScript.isPaused == false)
        {
            soundSFX.PlaySound(soundSFX.MainSFX,soundSFX.LightSwitchSFX);
        }

        if (Input.GetMouseButtonDown(1) && CanUseFlashLight == true && pauseScript.isPaused == false)
        {
            soundSFX.PlaySound(soundSFX.MainSFX, soundSFX.LightSwitchSFX);
        }

        if (Input.GetMouseButton(0) && CanUseFlashLight == true && pauseScript.isPaused == false)
        {
            flashFreeze();
            fLight.color = Color.blue;       
        }

        if (Input.GetMouseButton(1) && CanUseFlashLight == true && pauseScript.isPaused == false)
        {
            flashUnFreeze();
            fLight.color = Color.red;
        }

        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0))
        {
            fLight.color = Color.white;
        }
    }

    private void flashFreeze()
    {
        r.origin = gameObject.transform.position;
        r.direction = gameObject.transform.forward;


        if (Physics.Raycast(r, out hit, 100))
        {

            if (hit.transform.tag == "Capture" || hit.transform.tag == "Enemy")
            {
                rbg = hit.collider.gameObject.GetComponent<Rigidbody>();
                rbg.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                Debug.Log("No RigidBody");
            }

            if  (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<NavMeshAgent>().speed = 0;
                DamageTrigger.SetActive(false);
            }

        }
    }

    private void flashUnFreeze()
    {
        r.origin = gameObject.transform.position;
        r.direction = gameObject.transform.forward;

        if (Physics.Raycast(r, out hit, 100))
        {

            if (hit.transform.tag == "Capture")
            {
                rbg = hit.collider.gameObject.GetComponent<Rigidbody>();
                rbg.constraints = RigidbodyConstraints.None;
            }
            else if (rbg == null)
            {
                Debug.Log("No RigidBody");
            }

            if (hit.transform.tag == "Enemy")
            {
                hit.transform.gameObject.GetComponent<NavMeshAgent>().speed = 3.5f;
                DamageTrigger.SetActive(true);
            }

        }
    }
}