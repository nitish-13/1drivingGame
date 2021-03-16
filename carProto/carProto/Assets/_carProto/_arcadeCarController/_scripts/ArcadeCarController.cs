using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeCarController : MonoBehaviour
{
    // Start is called before the first frame update

    //get input
    //use input to move sphere

    private float moveInput;
    private float turnInput;
    private bool isCarGrounded;

    public float fwdSpeed;
    public float revSpeed;
    public float turnSpeed;
    public LayerMask groundLayer;
    public float airDrag;
    public float groundDrag;



    public Rigidbody sphereRB;



    void Start()
    {
        //detaches spehre from car
        sphereRB.transform.parent = null;

    }

    // Update is called once per frame
    void Update()
    {
        // get input
        moveInput = Input.GetAxisRaw("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");


        // adjust speed of the car
        moveInput *= moveInput > 0 ? fwdSpeed : revSpeed;

        // set car rotation
        //float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        //transform.Rotate(0, newRotation, 0, Space.World);
    
        // set car position to sphere
        transform.position = sphereRB.transform.position;

        // raycast ground check
        RaycastHit hit;
        isCarGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1f, groundLayer);

        // rotate car to be parallel to ground
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        if (isCarGrounded)
        {
            // set car rotation
            float newRotation = turnInput * turnSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
            transform.Rotate(0, newRotation, 0, Space.World);

            sphereRB.drag = groundDrag;
        }
        else
        {
            sphereRB.drag = airDrag;
        }
    }


    private void FixedUpdate()
    {
        if (isCarGrounded)
        {
            //move car
            sphereRB.AddForce(transform.forward * moveInput, ForceMode.Acceleration);
        }
        else
        {
            //add extra gravity
            sphereRB.AddForce(transform.up * -9.8f);
        }
    }
}
