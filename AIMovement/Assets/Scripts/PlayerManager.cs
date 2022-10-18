// The basic movement part was done by Kevin
// The advanced movement vas done by Johannes

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {
    private Rigidbody rb;
    public float speed = 4f;
    public float sprintSpeed = 9f;
    public float jumpForce = 400f;
    private float vertical;
    private float horizontal;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
     bool grounded;

     public float groundDrag;
    void Start() {

        rb = GetComponent<Rigidbody>();
    }  

    private void update()
    {
    
        // ground check 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);
        
        Speedcontrol();

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
            
    } 
   
    private void Speedcontrol()
    { 
    
       Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       // limit velocity if needed
       if(flatVel.magnitude > speed)
       {

          Vector3 limitedVel = flatVel.normalized * speed;
          rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); 
        }




    }

    void FixedUpdate() {
        float verticalPlayerInput = Input.GetAxisRaw(axisName: "Vertical"); //Gets vertical input
        float horizontalPlayerInput = Input.GetAxisRaw(axisName: "Horizontal"); //Gets horizontal input

        Vector3 forward = transform.InverseTransformVector (vector: Camera.main.transform.forward);
        Vector3 right = transform.InverseTransformVector (vector: Camera.main.transform.right);
        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        float speed = this.speed;
        Vector3 forwardRelativeVerticalInput = verticalPlayerInput * forward * Time.fixedDeltaTime;
        Vector3 rightRelativeHorizontalInput = horizontalPlayerInput * right * Time.fixedDeltaTime;

        Vector3 cameraRelativeMovement = forwardRelativeVerticalInput + rightRelativeHorizontalInput;
        if(Input.GetKey(key: KeyCode.LeftShift)) { //Sprinting 
            speed = sprintSpeed;
        }
        transform.Translate(translation: cameraRelativeMovement * speed, relativeTo: Space.World);

        if(grounded) {
            if (Input.GetButtonDown(buttonName: "Jump")) {
                rb.AddForce(force: Vector3.up * jumpForce);
            }
        }
    }

    //Should we be grounded?
    void OnCollisionEnter(Collision collision) {  // Yes
        if(collision.gameObject.tag == ("Ground")) {
            grounded = true;
        }
    }
    
    void OnCollisionExit(Collision collision) { // no
        if(collision.gameObject.tag == ("Ground")) {
            grounded = false;
        }
    }
}
