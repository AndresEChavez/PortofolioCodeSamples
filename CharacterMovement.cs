using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //getting gameObjects
    private Rigidbody rb;
    public GameObject PlayerCam;
    //setting values
    public float speed = 50;
    public float jump = 6;

    private float rotY = 0.0f;
    private float mouseX;
    public float inputSensitivity = 50.0f;
    //WASD and Vec3s
    private Vector3 forward;
    private Vector3 right;
    private Vector3 movementYes;
    private bool jumpIt = false;
    public AudioManager soundSFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.visible = false;

        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        soundSFX = GameObject.Find("Player").GetComponent<AudioManager>();
        forward = gameObject.transform.forward;
        right = gameObject.transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        mouseMovement();

        jumpPlease();

        //Debug.Log(rb.velocity.y);
    }

    private void FixedUpdate()
    {
        movement();

        if (jumpIt == true)
        {
            floor();
        }
    }

    //Jump
    void jumpPlease()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpIt = true;
        }
    }

    void floor()
    {
        RaycastHit hit;
        //Debug.DrawRay(transform.position, Vector3.down* raycastlength, Color.black);

        if(Physics.SphereCast(transform.position, 0.5f, Vector3.down, out hit, 1f))
        {
            if (hit.collider.gameObject && rb.velocity.y <= 0.01)
            {
                //Debug.Log(hit.collider.gameObject.name);
                rb.AddForce(0, jump , 0, ForceMode.Impulse);
                jumpIt = false;
                soundSFX.PlaySound(soundSFX.SecondSFX, soundSFX.JumpSFX);
            }
        } 
    }

    private void movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 tempVec = new Vector3(h, 0, v);
        tempVec = tempVec * speed * Time.deltaTime;
        tempVec = Camera.main.transform.TransformDirection(tempVec);
        tempVec.y = 0f;
        rb.MovePosition(transform.position + tempVec);
    }

    //Mouse
    private void mouseMovement()
    {
        mouseX = Input.GetAxis("Mouse X");
        rotY += mouseX * inputSensitivity * Time.deltaTime;
        Quaternion localRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
        transform.rotation = localRotation;
    }

    public void AdjustSensi(float newSens)
    {
        inputSensitivity = newSens;
    }
}
